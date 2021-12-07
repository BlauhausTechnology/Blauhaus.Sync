using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.ClientActors.Actors;
using Blauhaus.Common.Abstractions;
using Blauhaus.Responses;
using Blauhaus.Sync.Abstractions;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Client.SyncHandler;

namespace Blauhaus.Sync.Client
{
    public class SyncManager<TUser> : BaseActor, ISyncManager<TUser>
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IEnumerable<IDtoSyncHandler<TUser>> _dtoSyncHandlers;
        private OverallSyncStatus _overallStatus = null!;

        public SyncManager(
            IAnalyticsService analyticsService,
            IEnumerable<IDtoSyncHandler<TUser>> dtoSyncHandlers)
        {
            _analyticsService = analyticsService;
            _dtoSyncHandlers = dtoSyncHandlers;
        }

        public async Task<Response> SyncAllAsync(TUser? currentUser)
        {
            return await InvokeAsync(async () =>
            {
                using var _ = _analyticsService.StartTrace(this, "Sync");

                _overallStatus = new OverallSyncStatus();

                var dtoSyncClientTasks = new List<Task<Response>>();
                foreach (var dtoSyncClient in _dtoSyncHandlers)
                {
                    dtoSyncClientTasks.Add(SyncDtoAsync(dtoSyncClient, currentUser));
                }

                var syncResults = await Task.WhenAll(dtoSyncClientTasks);
                foreach (var syncResult in syncResults)
                {
                    if (syncResult.IsFailure) return Response.Failure(syncResult.Error);
                }

                return Response.Success();
            });
        }
         
        private async Task<Response> SyncDtoAsync(IDtoSyncHandler<TUser> dtoSyncHandler, TUser? currentUser)
        {
            var token = dtoSyncHandler.SubscribeAsync(async dtoSyncStatus =>
            {
                _overallStatus = _overallStatus.Update(dtoSyncStatus);
                await UpdateSubscribersAsync(_overallStatus);
            });

            var dtoSyncResult = await dtoSyncHandler.SyncDtoAsync(currentUser);

            token?.Dispose();

            return dtoSyncResult.IsFailure 
                ? Response.Failure(dtoSyncResult.Error) 
                : Response.Success();
        }

        public Task<IDisposable> SubscribeAsync(Func<IOverallSyncStatus, Task> handler, Func<IOverallSyncStatus, bool>? filter = null)
        {
            return Task.FromResult(AddSubscriber(handler, filter));
        }

    }
}