using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.ClientDatabase.Sqlite.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.Sync.Client.Sqlite;
using Blauhaus.Sync.Tests.TestObjects;
using SQLite;

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

        protected override AsyncTableQuery<MySyncedDtoEntity>? ApplyAdditionalFilters(AsyncTableQuery<MySyncedDtoEntity> query, IKeyValueProvider settingsProvider)
        {
            return ApplyAdditionalFilter ? query.Where(x => x.Name.Contains("Bill")) : base.ApplyAdditionalFilters(query, settingsProvider);
        }
         
        public bool ApplyAdditionalFilter { get; set; } 
    }
}