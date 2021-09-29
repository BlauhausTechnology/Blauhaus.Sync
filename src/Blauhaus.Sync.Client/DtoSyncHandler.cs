using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.ClientActors.Actors;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Responses;
using Blauhaus.Sync.Abstractions;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Abstractions.Common;
using Blauhaus.Sync.Client.SyncHandler;

namespace Blauhaus.Sync.Client
{
    public class DtoSyncHandler<TDto, TId> : BaseActor, IDtoSyncHandler
        where TDto : class, IClientEntity<TId>
        where TId : IEquatable<TId>
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ISyncDtoCache<TDto, TId> _syncDtoCache;
        private readonly ICommandHandler<Domain.Abstractions.Sync.DtoBatch<TDto, TId>, DtoSyncCommand> _syncCommandHandler;

        protected string DtoName = typeof(TDto).Name;

        public DtoSyncHandler(
            IAnalyticsService analyticsService,
            ISyncDtoCache<TDto, TId> syncDtoCache,
            ICommandHandler<Domain.Abstractions.Sync.DtoBatch<TDto, TId>, DtoSyncCommand> syncCommandHandler)
        {
            _analyticsService = analyticsService;
            _syncDtoCache = syncDtoCache;
            _syncCommandHandler = syncCommandHandler;
        }
         
        public Task<Response> SyncDtoAsync(IKeyValueProvider? settingsProvider)
        {
            return InvokeAsync(async () =>
            {
                var lastModifiedTicks = await _syncDtoCache.LoadLastModifiedTicksAsync(settingsProvider);

                var syncCommand = DtoSyncCommand.Create<TDto>(lastModifiedTicks);

                var syncResult = await _syncCommandHandler.HandleAsync(syncCommand);
                if (syncResult.IsFailure)
                {
                    return Response.Failure(syncResult.Error);
                }
                var dtoSyncStatus = DtoSyncStatus.Create(DtoName, syncResult.Value.CurrentDtoCount, syncResult.Value.RemainingDtoCount);

                await UpdateSubscribersAsync(dtoSyncStatus);

                while (dtoSyncStatus.RemainingDtoCount > 0)
                {
                    syncCommand = syncCommand.Update(syncResult.Value.BatchLastModifiedTicks);
                    syncResult = await _syncCommandHandler.HandleAsync(syncCommand);
                    if (syncResult.IsFailure)
                    {
                        return Response.Failure(syncResult.Error);
                    }

                    dtoSyncStatus = dtoSyncStatus.Update(syncResult.Value.CurrentDtoCount, syncResult.Value.RemainingDtoCount);
                    await UpdateSubscribersAsync(dtoSyncStatus);
                }

                return Response.Success();
            });

        }
        
        public Task<IDisposable> SubscribeAsync(Func<DtoSyncStatus, Task> handler, Func<DtoSyncStatus, bool>? filter = null)
        {
            return Task.FromResult(AddSubscriber(handler, filter));
        }

    }
}