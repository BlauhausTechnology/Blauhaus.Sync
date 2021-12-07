using System.Threading.Tasks;
using Blauhaus.Sync.Tests.Client.SyncDtoCacheTests.Base;
using NUnit.Framework;

namespace Blauhaus.Sync.Tests.Client.SyncDtoCacheTests
{
    public class DeleteOneAsyncTests : BaseSyncDtoCacheTest
    {
        [Test]
        public async Task IF_Dto_exists_SHOULD_delete()
        {
            //Arrange
            await Connection.InsertAsync(SyncedDtoEntityThree);
            
            //Act
            await Sut.DeleteOneAsync(DtoThree.Id);
            
            //Assert
            Assert.That(await Sut.TryGetOneAsync(DtoThree.Id), Is.Null);
        }
    }
}