using Blauhaus.TestHelpers;
using System;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.TestHelpers.MockBuilders;

namespace Blauhaus.Sync.TestHelpers.Extensions
{
    public static class MockContainerExtensions
    {
        
        public static Func<SyncDtoCacheMockBuilder<TDto, TId>> AddMockSyncDtoCache<TDto, TId>(this MockContainer mocks) where TDto : class, IClientEntity<TId> where TId : IEquatable<TId> 
            => mocks.AddMock<SyncDtoCacheMockBuilder<TDto, TId>, ISyncDtoCache<TDto,TId>>();
    }
}