using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.ClientDatabase.Sqlite.Config;
using Blauhaus.ClientDatabase.Sqlite.Service;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using Blauhaus.DeviceServices.TestHelpers.MockBuilders; 
using NUnit.Framework;
using SQLite;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Blauhaus.Sync.TestHelpers.Sqlite;
using Blauhaus.Sync.Tests.TestObjects;
using Blauhaus.Sync.Tests.TestObjects.User;

namespace Blauhaus.Sync.Tests.Client.Base
{ 
    public class SqliteConfig : BaseSqliteConfig
    {
        public SqliteConfig(IDeviceInfoService deviceInfoService) : base(deviceInfoService, "Test")
        {
            TableTypes = new List<Type>
            {
                typeof(MySyncedDtoEntity),
                typeof(MySyncedUserDtoEntity),

            };
        }
    }
}