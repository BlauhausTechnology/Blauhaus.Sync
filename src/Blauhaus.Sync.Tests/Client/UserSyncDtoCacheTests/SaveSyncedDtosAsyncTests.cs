using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Sync.Abstractions.Common;
using Blauhaus.Sync.Tests.Client.UserSyncDtoCacheTests.Base;
using Blauhaus.Sync.Tests.TestObjects;
using Blauhaus.Sync.Tests.TestObjects.User;
using NUnit.Framework;

namespace Blauhaus.Sync.Tests.Client.UserSyncDtoCacheTests
{
    public class SaveSyncedDtosAsyncTests : BaseSyncUserDtoCacheTest
    { 
        [Test]
        public async Task SHOULD_set_userId()
        {
            //Arrange
            var userId = Guid.NewGuid();
            DtoBuilders = new List<MyUserDtoBuilder> { new MyUserDtoBuilder().With(x => x.UserId, userId) };
            
            //Act
            var sut = ConstructSut();
            var result = await Sut.GetAllAsync();

            //Assert
            var loadedDto = result[0];
            Assert.That(loadedDto.UserId, Is.EqualTo(userId));
        }
    }
}