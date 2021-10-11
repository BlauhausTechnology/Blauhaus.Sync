using System;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.DtoHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.TestHelpers.EntityBuilders;
using Blauhaus.Sync.Server.EfCore.SyncHandlers;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Blauhaus.Sync.TestHelpers.EfCore
{
    public abstract class BaseUserDtoSyncCommandHandlerTest<TDbContext, TSut, TEntity, TEntityBuilder, TDto, TId, TUser, TUserMockBuilder> : BaseDtoSyncCommandHandlerTest<TDbContext, TSut, TEntity, TEntityBuilder, TDto, TId, TUser, TUserMockBuilder>
        where TSut : BaseUserDtoSyncCommandHandler<TDbContext, TEntity, TDto, TId, TUser> 
        where TDbContext : DbContext 
        where TEntity : class, IServerEntity, IDtoOwner<TDto>, IHasUserId
        where TDto : IClientEntity<TId>, IHasUserId
        where TId : IEquatable<TId>
        where TEntityBuilder : BaseServerEntityBuilder<TEntityBuilder, TEntity>
        where TUserMockBuilder : BaseMockBuilder<TUserMockBuilder, TUser>, new()
        where TUser : class, IHasUserId
    {
        public override void Setup()
        {
            base.Setup();

            foreach (var entityBuilder in EntitySet.Builders)
            {
                entityBuilder.With(x => x.UserId, User.UserId);
            }
        }

        [Test]
        public async Task SHOULD_exclude_entities_belonging_to_different_user()
        {
            //Arrange
            EntitySet.DistantPastEntityBuilder.With(x => x.UserId, Guid.NewGuid);

            //Act
            var result = await SyncAllAsync(0, User, Sut);

            //Assert
            Assert.That(result.Dtos.Count, Is.EqualTo(3));
            Assert.That(result.Dtos.FirstOrDefault(x => x.Id.Equals(EntitySet.DistantPastId)), Is.Null);
        }
    }
}