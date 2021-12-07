using Blauhaus.Common.Abstractions;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Blauhaus.Responses;
using Blauhaus.Sync.Abstractions.Client;
using Moq;

namespace Blauhaus.Sync.TestHelpers.MockBuilders
{
    public class SyncManagerMockBuilder : BaseAsyncPublisherMockBuilder<SyncManagerMockBuilder, ISyncManager, IOverallSyncStatus>
    {
        public SyncManagerMockBuilder Where_SyncAllAsync_returns(Response value)
        {
            Mock.Setup(x => x.SyncAllAsync(It.IsAny<IKeyValueProvider?>()))
                .ReturnsAsync(value);
            return this;
        }
         
    }
}