﻿using Blauhaus.Analytics.Abstractions.Service;
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
using Blauhaus.Sync.Tests.TestObjects;

namespace Blauhaus.Sync.Tests.Client.Base
{
    public abstract class BaseSqliteTest<TSut> : BaseClientSyncTest<TSut> where TSut : class
    {
        protected ISqliteDatabaseService SqliteDatabaseService = null!;
        protected SQLiteAsyncConnection Connection = null!;

        public override void Setup()
        {
            base.Setup();

            SqliteDatabaseService = new SqliteInMemoryDatabase(new SqliteConfig(MockDeviceInfoService.Object));
            Task.Run(async () => await SqliteDatabaseService.DeleteDataAsync()).Wait();
            Connection = SqliteDatabaseService.AsyncConnection;

            AddService(SqliteDatabaseService);
        }

        protected DeviceInfoServiceMockBuilder MockDeviceInfoService => AddMock<DeviceInfoServiceMockBuilder, IDeviceInfoService>().Invoke();
    }


    public class SqliteConfig : BaseSqliteConfig
    {
        public SqliteConfig(IDeviceInfoService deviceInfoService) : base(deviceInfoService, "Test")
        {
            TableTypes = new List<Type>
            {
                typeof(MySyncedDtoEntity),

            };
        }
    }
}