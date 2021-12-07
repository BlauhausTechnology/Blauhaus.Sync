using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Common;
using Blauhaus.Sync.Tests.Client.UserSyncDtoCacheTests.Base;
using Blauhaus.Sync.Tests.TestObjects;
using Blauhaus.Sync.Tests.TestObjects.User;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Sync.Tests.Client.UserSyncDtoCacheTests
{
    public class LoadLastModifiedAsyncTests : BaseSyncUserDtoCacheTest
    {
        [Test]
        public async Task IF_userId_is_available_SHOULD_ignore_entities_with_other_UserIds()
        {
            //Arrange
            var userId = Guid.NewGuid(); 
            MockKeyValueProvider.Where_TryGetValue_returns(userId.ToString(), "UserId");

            DtoBuilders = new List<MyUserDtoBuilder>
            {
                new MyUserDtoBuilder()
                    .With(x => x.UserId, userId)
                    .With(x => x.EntityState, EntityState.Active)
                    .With(x => x.ModifiedAtTicks, 1000),

                new MyUserDtoBuilder()
                    .With(x => x.UserId, Guid.NewGuid())
                    .With(x => x.EntityState, EntityState.Active)
                    .With(x => x.ModifiedAtTicks, 2000),
            };

            //Act
            var result = await Sut.LoadLastModifiedTicksAsync(MockKeyValueProvider.Object);

            //Assert
            Assert.That(result, Is.EqualTo(1000));
        }

        [Test]
        public async Task IF_no_userId_is_available_SHOULD_return_null()
        {
            //Arrange
            MockKeyValueProvider.Where_TryGetValue_returns(null, "UserId");
             
            //Act
            var result = await Sut.LoadLastModifiedTicksAsync(MockKeyValueProvider.Object);

            //Assert
            Assert.That(result, Is.Null);
        }
         
    }
}