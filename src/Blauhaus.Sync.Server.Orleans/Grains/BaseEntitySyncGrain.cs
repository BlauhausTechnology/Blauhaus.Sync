using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Domain.Abstractions.Commands;
using Blauhaus.Domain.Abstractions.DtoHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.Server.Entities;
using Blauhaus.Orleans.Abstractions.Resolver;
using Blauhaus.Orleans.EfCore.Grains;
using Blauhaus.Responses;
using Blauhaus.Sync.Server.Orleans.Abstractions;
using Blauhaus.Time.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Blauhaus.Sync.Server.Orleans.Grains
{
    public abstract class BaseEntitySyncGrain<TDbContext, TEntity, TDto, TDtoSyncGrain, TGrainResolver> : BaseEntityGrain<TDbContext, TEntity, TDto, TGrainResolver>,
        IVoidAuthenticatedCommandHandler<ActivateCommand, IAuthenticatedUser>,
        IVoidAuthenticatedCommandHandler<ArchiveCommand, IAuthenticatedUser>
        where TDbContext : DbContext
        where TEntity : BaseServerEntity, IDtoOwner<TDto>
        where TDto : IClientEntity<Guid>
        where TGrainResolver : IGrainResolver
        where TDtoSyncGrain : IDtoSyncGrain<TDto>
    {

        protected TDtoSyncGrain DtoSyncGrain = default!;

        protected BaseEntitySyncGrain(
            Func<TDbContext> dbContextFactory,
            IAnalyticsService analyticsService,
            ITimeService timeService,
            TGrainResolver grainResolver)
            : base(dbContextFactory, analyticsService, timeService, grainResolver)
        {
        }


        public override Task OnActivateAsync()
        {
            DtoSyncGrain = GrainResolver.ResolveSingleton<TDtoSyncGrain>();
            return base.OnActivateAsync();
        }
         
        protected override async Task<Response> HandleActivatedAsync(TEntity loadedEntity)
        {
            var dto = await LoadedEntity.GetDtoAsync();
            await DtoSyncGrain.UpdateDtoAsync(dto);
            return Response.Success();
        }

        protected override async Task<Response> HandleArchivedAsync(TEntity entity)
        {
            var dto = await LoadedEntity.GetDtoAsync();
            await DtoSyncGrain.UpdateDtoAsync(dto);
            return Response.Success();
        }

        protected override async Task<Response> HandleDeletedAsync(TEntity entity)
        {
            var dto = await LoadedEntity.GetDtoAsync();
            await DtoSyncGrain.UpdateDtoAsync(dto);
            return Response.Success();
        }
    }

}