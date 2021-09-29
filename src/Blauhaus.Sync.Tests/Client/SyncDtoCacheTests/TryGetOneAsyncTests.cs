﻿using System.Threading.Tasks;
using Blauhaus.Sync.Tests.Client.SyncDtoCacheTests.Base;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Blauhaus.Sync.Tests.Client.SyncDtoCacheTests
{
    public class TryGetOneAsyncTests : BaseSyncDtoCacheTest
    {

        [Test]
        public async Task IF_Dto_exists_SHOULD_return()
        {
            //Arrange
            await Connection.InsertAsync(SyncedDtoEntityThree);
            
            //Act
            var result = await Sut.TryGetOneAsync(DtoThree.Id);
            
            //Assert
            Assert.That(JsonConvert.SerializeObject(result), Is.EqualTo(JsonConvert.SerializeObject(DtoThree)));
        }
        
        [Test]
        public async Task IF_Dto_does_not_existS_SHOULD_return_null()
        {
            //Arrange
            await Connection.InsertAsync(SyncedDtoEntityThree);
            
            //Act
            var result = await Sut.TryGetOneAsync(DtoOne.Id);
            
            //Assert
            Assert.That(result, Is.Null);
        }
    }
}