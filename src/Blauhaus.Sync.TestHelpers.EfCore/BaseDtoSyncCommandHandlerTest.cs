using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.Domain.Abstractions.DtoHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.TestHelpers.EFCore.DbContextBuilders;
using Blauhaus.Domain.TestHelpers.EntityBuilders;
using Blauhaus.Sync.Abstractions.Common;
using Blauhaus.Sync.Server.EfCore.SyncHandlers;
using Blauhaus.TestHelpers.BaseTests;
using Blauhaus.TestHelpers.MockBuilders;
using Blauhaus.Time.TestHelpers.MockBuilders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using EntityState = Blauhaus.Domain.Abstractions.Entities.EntityState;

namespace Blauhaus.Sync.TestHelpers.EfCore
{
    public abstract class BaseDtoSyncCommandHandlerTest<TDbContext, TSut, TEntity, TEntityBuilder, TDto, TId, TUser, TUserMockBuilder> : BaseServiceTest<TSut> 
        where TSut : BaseDtoSyncCommandHandler<TDbContext, TEntity, TDto, TId, TUser> 
        where TDbContext : DbContext 
        where TEntity : class, IServerEntity, IDtoOwner<TDto> 
        where TDto : IClientEntity<TId> 
        where TId : IEquatable<TId>
        where TEntityBuilder : BaseServerEntityBuilder<TEntityBuilder, TEntity>
        where TUserMockBuilder : BaseMockBuilder<TUserMockBuilder, TUser>, new()
        where TUser : class
    {
         
        private InMemoryDbContextBuilder<TDbContext> _dbContextBuilder = null!;

        protected EntitySyncSet<TEntity, TEntityBuilder> EntitySet = null!;

        protected TUserMockBuilder MockUser = null!;
        protected TUser User = null!;

        protected AnalyticsServiceMockBuilder MockAnalyticsService = null!;
        protected TimeServiceMockBuilder MockTimeService = null!;

        protected DateTime RunTime;

        [SetUp]
        public virtual void Setup()
        {
            base.Cleanup();
            
            _dbContextBuilder = new InMemoryDbContextBuilder<TDbContext>();
            TDbContext FactoryFunc() => _dbContextBuilder.NewContext;
            Services.AddSingleton<Func<TDbContext>>(FactoryFunc);

            MockAnalyticsService = new AnalyticsServiceMockBuilder();
            AddService(MockAnalyticsService.Object);
            
            MockTimeService = new TimeServiceMockBuilder();
            AddService(MockTimeService.Object);

            MockUser = new TUserMockBuilder();
            User = MockUser.Object;
            
            var setupTime = MockTimeService.Reset();
            RunTime = setupTime.AddSeconds(122);

            EntitySet = new EntitySyncSet<TEntity, TEntityBuilder>(RunTime);
        }


        protected override TSut ConstructSut()
        {
            using (var db = _dbContextBuilder.NewContext)
            {
                foreach (var builder in EntitySet.Builders)
                {
                    db.Add(builder.Object);
                }
                db.SaveChanges();
            }

            return base.ConstructSut();
        }

        [Test]
        public async Task WHEN_IsFirstSync_SHOULD_return_all_Active()
        { 
            //Arrange
            Sut.MaxBatchSize = 1;

            //Act
            var result = await SyncAllAsync(0, User, Sut);

            //Assert
            Assert.That(result.Dtos.Count, Is.EqualTo(4));
            Assert.That(result.DtoBatches.Count, Is.EqualTo(4));
            Assert.That(result.Dtos.All(x => x.EntityState == EntityState.Active), Is.True);
            Assert.That(result.Dtos[0].ModifiedAtTicks, Is.EqualTo(EntitySet.DistantPastTime.Ticks));
            Assert.That(result.Dtos[1].ModifiedAtTicks, Is.EqualTo(EntitySet.PastTime.Ticks));
            Assert.That(result.Dtos[2].ModifiedAtTicks, Is.EqualTo(EntitySet.PresentTime.Ticks));
            Assert.That(result.Dtos[3].ModifiedAtTicks, Is.EqualTo(EntitySet.FutureTime.Ticks));
        }

        [Test]
        public async Task WHEN_ModifiedAfter_is_given_SHOULD_return_modified_including_Deleted()
        { 
            //Arrange
            Sut.MaxBatchSize = 1;

            //Act
            var result = await SyncAllAsync(EntitySet.PastTime.Ticks, User, Sut);

            //Assert
            Assert.That(result.Dtos.Count, Is.EqualTo(3));
            Assert.That(result.DtoBatches.Count, Is.EqualTo(3));
            Assert.That(result.Dtos[0].ModifiedAtTicks, Is.EqualTo(EntitySet.PresentTime.Ticks));
            Assert.That(result.Dtos[1].ModifiedAtTicks, Is.EqualTo(EntitySet.FutureTime.Ticks));
            Assert.That(result.Dtos[2].ModifiedAtTicks, Is.EqualTo(EntitySet.FutureDeletedTime.Ticks));
        }


        [Test]
        public async Task WHEN_there_are_no_modified_entities_SHOULD_return_empty()
        { 
            //Arrange
            Sut.MaxBatchSize = 1;

            //Act
            var result = await SyncAllAsync(EntitySet.FutureDeletedTime.Ticks, User, Sut);

            //Assert
            Assert.That(result.Dtos.Count, Is.EqualTo(0));
            Assert.That(result.DtoBatches.Count, Is.EqualTo(1));
        }


        [Test]
        public async Task SHOULD_apply_batch_size()
        {
            //Arrange
            Sut.MaxBatchSize = 2;

            //Act
            var result = await SyncAllAsync(0, User, Sut);

            //Assert
            Assert.That(result.Dtos.Count, Is.EqualTo(4));
            Assert.That(result.DtoBatches.Count, Is.EqualTo(2));
            Assert.That(result.Dtos.All(x => x.EntityState == EntityState.Active), Is.True);
            Assert.That(result.Dtos[0].ModifiedAtTicks, Is.EqualTo(EntitySet.DistantPastTime.Ticks));
            Assert.That(result.Dtos[1].ModifiedAtTicks, Is.EqualTo(EntitySet.PastTime.Ticks));
            Assert.That(result.Dtos[2].ModifiedAtTicks, Is.EqualTo(EntitySet.PresentTime.Ticks));
            Assert.That(result.Dtos[3].ModifiedAtTicks, Is.EqualTo(EntitySet.FutureTime.Ticks));
        }


        protected async Task<SyncTestResult<TDto, TId>> SyncAllAsync(long lastModified, TUser user, TSut handler)
        {
            var results = new List<DtoBatch<TDto, TId>>();

            var syncCommand = DtoSyncCommand.Create<TDto>(lastModified);
            var syncResult = await handler.HandleAsync(syncCommand, user);
            results.Add(syncResult.Value);

            while (syncResult.Value.RemainingDtoCount > 0)
            {
                syncCommand = syncCommand.Update(syncResult.Value.BatchLastModifiedTicks);
                syncResult = await handler.HandleAsync(syncCommand, user);
                results.Add(syncResult.Value);
            }

            return new SyncTestResult<TDto, TId>(results);
        }
    }
}