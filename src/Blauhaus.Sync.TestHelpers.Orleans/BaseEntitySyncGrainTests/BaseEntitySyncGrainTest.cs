using System;
using Blauhaus.Domain.Abstractions.DtoHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.Server.Entities;
using Blauhaus.Domain.TestHelpers.EntityBuilders;
using Blauhaus.Orleans.Abstractions.Resolver;
using Blauhaus.Orleans.EfCore.Grains;
using Blauhaus.Orleans.TestHelpers.BaseTests;
using Blauhaus.Orleans.TestHelpers.MockBuilders.Resolver;
using Blauhaus.Sync.Server.Orleans.Abstractions;
using Blauhaus.Sync.Server.Orleans.Grains;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.EntityFrameworkCore;

namespace Blauhaus.Sync.TestHelpers.Orleans.BaseEntitySyncGrainTests
{
    public abstract class BaseEntitySyncGrainTest<TDbContext, TGrain, TEntity, TEntityBuilder, TDto, TDtoSyncGrain, TGrainResolver, TGrainResolverMockBuilder> 
        : BaseEntityGrainTest<TDbContext, TGrain, TEntity, TEntityBuilder, TGrainResolver> 
        where TDbContext : DbContext 
        where TGrain : BaseEntitySyncGrain<TDbContext, TEntity, TDto, TDtoSyncGrain, TGrainResolver> 
        where TEntity : BaseServerEntity, IDtoOwner<TDto>
        where TEntityBuilder : BaseServerEntityBuilder<TEntityBuilder, TEntity> 
        where TGrainResolver : class, IGrainResolver
        where TDto : IClientEntity<Guid>
        where TDtoSyncGrain : class, IDtoSyncGrain<TDto>
        where TGrainResolverMockBuilder : BaseGrainResolverMockBuilder<TGrainResolverMockBuilder, TGrainResolver>, new()
    {

        protected MockBuilder<TDtoSyncGrain> MockDtoSyncGrain = null!;
        protected TGrainResolverMockBuilder MockGrainResolver = null!;

        public override void Setup()
        {
            base.Setup();

            MockDtoSyncGrain = new MockBuilder<TDtoSyncGrain>();
            
            MockGrainResolver = new TGrainResolverMockBuilder();
            MockGrainResolver.Mock.Setup(x => x.ResolveSingleton<TDtoSyncGrain>())
                .Returns(MockDtoSyncGrain.Object);
            AddSiloService(MockGrainResolver.Object);

        }
    }
}