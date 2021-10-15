using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.ClientDatabase.Sqlite.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.Sync.Client.Sqlite.DtoCaches;
using Blauhaus.Sync.Tests.TestObjects;
using Blauhaus.Sync.Tests.TestObjects.User;
using SQLite;

namespace Blauhaus.Sync.Tests.Client.UserSyncDtoCacheTests.Base
{
    public class TestSyncUserDtoCache : SyncUserDtoCache<MyUserDto, MySyncedUserDtoEntity, Guid>
    {
        public TestSyncUserDtoCache(IAnalyticsService analyticsService, ISqliteDatabaseService sqliteDatabaseService) : base(analyticsService, sqliteDatabaseService)
        {
        }
         
    }
}