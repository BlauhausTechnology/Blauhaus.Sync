using Blauhaus.ClientDatabase.Sqlite.Config;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using System.Collections.Generic;
using System;
using Blauhaus.Sync.Tests.Client.TestObjects;
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