using Blauhaus.Common.Abstractions;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Blauhaus.Responses;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Client.SyncHandler;
using Moq;

namespace Blauhaus.Sync.TestHelpers.MockBuilders
{
    public class DtoSyncClientMockBuilder : DtoSyncClientMockBuilder<DtoSyncClientMockBuilder, IDtoSyncHandler>
    {
    }

    public abstract class DtoSyncClientMockBuilder<TBuilder, TMock> : BaseAsyncPublisherMockBuilder<TBuilder, TMock, DtoSyncStatus>
        where TBuilder: DtoSyncClientMockBuilder<TBuilder, TMock>
        where TMock : class, IDtoSyncHandler
    {

        public TBuilder Where_SyncDtoAsync_returns(Response value)
        {
            Mock.Setup(x => x.SyncDtoAsync(It.IsAny<IKeyValueProvider?>()))
                .ReturnsAsync(value);
            return (TBuilder)this;
        }  


    }
}