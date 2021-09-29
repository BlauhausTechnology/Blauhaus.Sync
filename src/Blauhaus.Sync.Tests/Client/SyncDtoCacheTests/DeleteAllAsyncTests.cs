using System.Threading.Tasks;
using Blauhaus.Sync.Tests.Client.SyncDtoCacheTests.Base;
using NUnit.Framework;

namespace Blauhaus.Sync.Tests.Client.SyncDtoCacheTests
{
    public class DeleteAllAsyncTests : BaseSyncDtoCacheTest
    {
        [Test]
        public async Task SHOULD_delete_all()
        {
            //Arrange
            await Connection.InsertAsync(SyncedDtoEntityOne);
            await Connection.InsertAsync(SyncedDtoEntityTwo);
            await Connection.InsertAsync(SyncedDtoEntityThree);
            
            //Act
            await Sut.DeleteAllAsync();
            
            //Assert
            Assert.That((await Sut.GetAllAsync()).Count, Is.EqualTo(0));
        }
    }
}