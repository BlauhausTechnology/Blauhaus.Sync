using Blauhaus.Common.Abstractions;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Blauhaus.Domain.Abstractions.Sync;
using Blauhaus.Responses;
using Moq;

namespace Blauhaus.Sync.TestHelpers
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