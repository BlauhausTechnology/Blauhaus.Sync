using Blauhaus.Common.Abstractions;
using Blauhaus.Sync.Client;
using Blauhaus.Sync.TestHelpers;
using Blauhaus.Sync.TestHelpers.MockBuilders;
using Blauhaus.Sync.Tests.Client.Base;
using Blauhaus.Sync.Tests.Client.TestObjects;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.Sync.Tests.Client.SyncManagerTests.Base
{
    public abstract class BaseSyncManagerTest : BaseClientSyncTest<SyncManager<MyTestUser>>
    {

        protected DtoSyncClientMockBuilder<MyTestUser> MockSyncClient1 = null!;
        protected DtoSyncClientMockBuilder<MyTestUser> MockSyncClient2 = null!;
        protected DtoSyncClientMockBuilder<MyTestUser> MockSyncClient3 = null!;
        
        protected MyTestUser TestUser = new();

        public override void Setup()
        {
            base.Setup();

            MockSyncClient1 = new DtoSyncClientMockBuilder<MyTestUser>();
            MockSyncClient2 = new DtoSyncClientMockBuilder<MyTestUser>();
            MockSyncClient3 = new DtoSyncClientMockBuilder<MyTestUser>();

            AddService(MockSyncClient1.Object);
            AddService(MockSyncClient2.Object);
            AddService(MockSyncClient3.Object);
        }
    }
}