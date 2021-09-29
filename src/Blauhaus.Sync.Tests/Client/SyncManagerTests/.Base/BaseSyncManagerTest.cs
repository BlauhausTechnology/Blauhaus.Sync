using Blauhaus.Common.Abstractions;
using Blauhaus.Sync.Client;
using Blauhaus.Sync.TestHelpers;
using Blauhaus.Sync.TestHelpers.MockBuilders;
using Blauhaus.Sync.Tests.Client.Base;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.Sync.Tests.Client.SyncManagerTests.Base
{
    public abstract class BaseSyncManagerTest : BaseClientSyncTest<SyncManager>
    {

        protected DtoSyncClientMockBuilder MockSyncClient1 = null!;
        protected DtoSyncClientMockBuilder MockSyncClient2 = null!;
        protected DtoSyncClientMockBuilder MockSyncClient3 = null!;
        
        protected IKeyValueProvider MockKeyValueProvider = new MockBuilder<IKeyValueProvider>().Object;

        public override void Setup()
        {
            base.Setup();

            MockSyncClient1 = new DtoSyncClientMockBuilder();
            MockSyncClient2 = new DtoSyncClientMockBuilder();
            MockSyncClient3 = new DtoSyncClientMockBuilder();

            AddService(MockSyncClient1.Object);
            AddService(MockSyncClient2.Object);
            AddService(MockSyncClient3.Object);
        }
    }
}