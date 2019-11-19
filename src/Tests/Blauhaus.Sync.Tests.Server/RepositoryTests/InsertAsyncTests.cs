using System.Runtime.Serialization;
using System.Threading.Tasks;
using Blauhaus.Common.TestHelpers;
using Blauhaus.Sync.Server.Repository.Repository;
using Blauhaus.Sync.Tests.Server.TestObjects;
using NUnit.Framework;

namespace Blauhaus.Sync.Tests.Server.RepositoryTests
{
    [TestFixture]
    public class InsertAsyncTests : BaseUnitTest<ServerRepository<AppleEntity>>
    {
        protected override ServerRepository<AppleEntity> ConstructSut()
        {
            return new ServerRepository<AppleEntity>("Data Source=:memory:;Version=3;New=True;");
        }

        [Test]
        public async Task SHOULD_add_item()
        {
            //Arrange
            var apple = new AppleEntity{Colour = "Red"};

            //Act
            await Sut.InsertAsync(apple);

            //Assert
            var loadedApple = await Sut.LoadAsync(apple.Id);
            Assert.That(loadedApple.Colour, Is.EqualTo("Red"));
        }
    }
}