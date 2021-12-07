using System;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.TestHelpers.MockBuilders.Client.DtoCaches;
using Blauhaus.Sync.Abstractions.Client;
using Moq;

namespace Blauhaus.Sync.TestHelpers.MockBuilders
{

    public class SyncDtoCacheMockBuilder<TDto, TId, TUser> : BaseSyncDtoCacheMockBuilder<SyncDtoCacheMockBuilder<TDto, TId, TUser>, ISyncDtoCache<TDto, TId, TUser>, TDto, TId, TUser>
        where TDto : class, IClientEntity<TId> where TId : IEquatable<TId> where TUser : IHasId<TId>
    {

    }

    public abstract class BaseSyncDtoCacheMockBuilder<TBuilder, TMock, TDto, TId, TUser> : BaseDtoCacheMockBuilder<TBuilder, TMock, TDto, TId>
        where TBuilder : BaseSyncDtoCacheMockBuilder<TBuilder, TMock, TDto, TId, TUser> 
        where TMock : class, ISyncDtoCache<TDto, TId, TUser>
        where TDto : class, IClientEntity<TId>
        where TId : IEquatable<TId>
        where TUser : IHasId<TId>
    {

        public TBuilder Where_LoadLastModifiedTicksAsync_returns(long? value)
        {
            Mock.Setup(x => x.LoadLastModifiedTicksAsync(It.IsAny<TUser?>()))
                .ReturnsAsync(value);
            return (TBuilder)this;
        }
    }
}