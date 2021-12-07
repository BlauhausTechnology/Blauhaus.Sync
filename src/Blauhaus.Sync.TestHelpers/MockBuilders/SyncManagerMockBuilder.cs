using Blauhaus.Common.Abstractions;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Blauhaus.Responses;
using Blauhaus.Sync.Abstractions.Client;
using Moq;

namespace Blauhaus.Sync.TestHelpers.MockBuilders
{
    public class SyncManagerMockBuilder<TUser> : BaseAsyncPublisherMockBuilder<SyncManagerMockBuilder<TUser>, ISyncManager<TUser>, IOverallSyncStatus>
    {
        public SyncManagerMockBuilder<TUser> Where_SyncAllAsync_returns(Response value)
        {
            Mock.Setup(x => x.SyncAllAsync(It.IsAny<TUser?>()))
                .ReturnsAsync(value);
            return this;
        }
         
    }
}