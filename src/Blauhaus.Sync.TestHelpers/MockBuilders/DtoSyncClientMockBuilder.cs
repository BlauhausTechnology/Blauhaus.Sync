using System;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Blauhaus.Responses;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Client.SyncHandler;
using Moq;

namespace Blauhaus.Sync.TestHelpers.MockBuilders
{
    public class DtoSyncClientMockBuilder<TUser> : DtoSyncClientMockBuilder<DtoSyncClientMockBuilder<TUser>, IDtoSyncHandler<TUser>, TUser>
    {
    }

    public abstract class DtoSyncClientMockBuilder<TBuilder, TMock, TUser> : BaseAsyncPublisherMockBuilder<TBuilder, TMock, DtoSyncStatus>
        where TBuilder: DtoSyncClientMockBuilder<TBuilder, TMock, TUser>
        where TMock : class, IDtoSyncHandler<TUser>
    {

        public TBuilder Where_SyncDtoAsync_returns(Response value)
        {
            Mock.Setup(x => x.SyncDtoAsync(It.IsAny<TUser?>()))
                .ReturnsAsync(value);
            return (TBuilder)this;
        }  


    }
}