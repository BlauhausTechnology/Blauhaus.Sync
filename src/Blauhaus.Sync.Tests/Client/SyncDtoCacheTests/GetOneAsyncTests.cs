using System.Threading.Tasks;
using Blauhaus.Errors;
using Blauhaus.Sync.Tests.Client.SyncDtoCacheTests.Base;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Blauhaus.Sync.Tests.Client.SyncDtoCacheTests
{
    public class GetOneAsyncTests : BaseSyncDtoCacheTest
    {

        [Test]
        public async Task IF_Dto_exists_SHOULD_return()
        {
            //Arrange
            await Connection.InsertAsync(SyncedDtoEntityThree);
            
            //Act
            var result = await Sut.GetOneAsync(DtoThree.Id);
            
            //Assert
            Assert.That(JsonConvert.SerializeObject(result), Is.EqualTo(JsonConvert.SerializeObject(DtoThree)));
        }
        
        [Test]
        public void IF_Dto_does_not_existS_SHOULD_throw()
        { 
            //Act
            Assert.ThrowsAsync<ErrorException>(async ()=> await Sut.GetOneAsync(DtoOne.Id));
        }
         
    }
}