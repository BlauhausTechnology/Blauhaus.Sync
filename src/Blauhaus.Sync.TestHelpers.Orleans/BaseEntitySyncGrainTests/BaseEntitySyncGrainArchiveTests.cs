using System;
using System.Threading.Tasks;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Domain.Abstractions.Commands;
using Blauhaus.Domain.Abstractions.DtoHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.Server.Entities;
using Blauhaus.Domain.TestHelpers.EntityBuilders;
using Blauhaus.Orleans.Abstractions.Resolver;
using Blauhaus.Orleans.TestHelpers.BaseEntityGrainTests;
using Blauhaus.Sync.Server.Orleans.Abstractions;
using Blauhaus.Sync.Server.Orleans.Grains;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using EntityState = Blauhaus.Domain.Abstractions.Entities.EntityState;

namespace Blauhaus.Sync.TestHelpers.Orleans.BaseEntitySyncGrainTests
{
    public abstract class BaseEntitySyncGrainArchiveTests<TDbContext, TGrain, TEntity, TEntityBuilder, TGrainResolver, TDto, TSyncGrain> 
        : BaseEntityGrainArchiveTests<TDbContext, TGrain, TEntity, TEntityBuilder, TGrainResolver, TDto>
        where TGrain: BaseEntitySyncGrain<TDbContext, TEntity, TDto, TSyncGrain, TGrainResolver>, IVoidAuthenticatedCommandHandler<ArchiveCommand, IAuthenticatedUser> 
        where TEntity : BaseServerEntity, IDtoOwner<TDto>
        where TEntityBuilder : BaseServerEntityBuilder<TEntityBuilder, TEntity>
        where TDbContext : DbContext
        where TSyncGrain : class, IDtoSyncGrain<TDto>
        where TDto : IClientEntity<Guid>
        where TGrainResolver : class, IGrainResolver
    {

        protected MockBuilder<TSyncGrain> MockDtoSyncGrain = null!;

        public override void Setup()
        {
            base.Setup();

            MockDtoSyncGrain = new MockBuilder<TSyncGrain>();
            
            var mockGrainResolver = new MockBuilder<TGrainResolver>();
            mockGrainResolver.Mock.Setup(x => x.ResolveSingleton<TSyncGrain>()).Returns(MockDtoSyncGrain.Object);
            AddSiloService(mockGrainResolver.Object);
        }
        
        [TestCase(EntityState.Active)]
        public async Task IF_Active_SHOULD_Activate_and_publish(EntityState validState)
        {
            //Arrange
            ExistingEntityBuilder.With(x => x.EntityState, validState);

            //Act
            var result = await Sut.HandleAsync(Command, AdminUser);

            //Assert
            Assert.That(result.IsSuccess); 
            MockDtoSyncGrain.Mock.Verify(x => x.UpdateDtoAsync(It.Is<TDto>(y =>
                y.Id == GrainId &&
                y.EntityState == EntityState.Archived)));
        }
    }
}