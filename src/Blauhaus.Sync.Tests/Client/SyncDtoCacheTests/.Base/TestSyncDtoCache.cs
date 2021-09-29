using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.ClientDatabase.Sqlite.Service;
using Blauhaus.Domain.Client.Sqlite.DtoCaches;
using Blauhaus.Sync.Tests.Client.TestObjects;
using Blauhaus.Sync.Tests.TestObjects;

namespace Blauhaus.Sync.Tests.Client.SyncDtoCacheTests.Base
{
    public class TestSyncDtoCache : SyncDtoCache<MyDto, MySyncedDtoEntity, Guid>
    {
        public TestSyncDtoCache(IAnalyticsService analyticsService, ISqliteDatabaseService sqliteDatabaseService) : base(analyticsService, sqliteDatabaseService)
        {
        }

        protected override async Task<MySyncedDtoEntity> PopulateEntityAsync(MyDto dto)
        {
            var entity = await base.PopulateEntityAsync(dto);
            entity.Name = dto.Name;
            return entity;
        }
    }
}