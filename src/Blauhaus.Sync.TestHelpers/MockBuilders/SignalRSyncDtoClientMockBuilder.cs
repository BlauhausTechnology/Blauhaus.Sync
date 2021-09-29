using System;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Client.SignalR;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.Sync.TestHelpers.MockBuilders
{
    public class SignalRSyncDtoClientMockBuilder<TDto, TId> : BaseSignalRSyncDtoClientMockBuilder<SignalRSyncDtoClientMockBuilder<TDto, TId>, ISignalRSyncDtoClient<TDto, TId>, TDto, TId> 
        where TDto : class, IClientEntity<TId> where TId : IEquatable<TId>
    {
    }

    public abstract class BaseSignalRSyncDtoClientMockBuilder<TBuilder, TMock, TDto, TId> : BaseMockBuilder<TBuilder, TMock>
        where TBuilder : BaseSignalRSyncDtoClientMockBuilder<TBuilder, TMock, TDto, TId> 
        where TDto : class, IClientEntity<TId>
        where TMock : class, ISignalRSyncDtoClient<TDto, TId>
        where TId : IEquatable<TId>
    {
    }
}