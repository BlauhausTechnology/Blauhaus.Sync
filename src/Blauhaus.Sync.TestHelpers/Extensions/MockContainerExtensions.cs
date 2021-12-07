using Blauhaus.TestHelpers;
using System;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.TestHelpers.MockBuilders;

namespace Blauhaus.Sync.TestHelpers.Extensions
{
    public static class MockContainerExtensions
    {
        
        public static Func<SyncDtoCacheMockBuilder<TDto, TId, TUser>> AddMockSyncDtoCache<TDto, TId, TUser>(this MockContainer mocks) 
            where TDto : class, IClientEntity<TId> 
            where TId : IEquatable<TId> 
            where TUser : IHasId<TId> 
                => mocks.AddMock<SyncDtoCacheMockBuilder<TDto, TId, TUser>, ISyncDtoCache<TDto,TId, TUser>>();
    }
}