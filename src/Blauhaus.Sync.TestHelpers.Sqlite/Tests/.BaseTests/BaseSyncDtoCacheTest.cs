using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientDatabase.Sqlite.Config;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Common;
using Blauhaus.Sync.Client.Sqlite.DtoCaches;
using Blauhaus.Sync.Client.Sqlite.Entities;
using Blauhaus.TestHelpers.Builders.Base;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.Sync.TestHelpers.Sqlite.Tests.BaseTests
{
    public abstract class BaseSyncDtoCacheTest<TSut, TDto, TDtoBuilder, TEntity, TId, TConfig, TUser>  : BaseSqliteTest<TSut, TConfig>
        where TSut : SyncDtoCache<TDto, TEntity, TId, TUser> 
        where TDto : class, IClientEntity<TId>
        where TDtoBuilder : IBuilder<TDtoBuilder, TDto>
        where TEntity : BaseSyncClientEntity<TId>, new()
        where TId : IEquatable<TId>
        where TConfig : BaseSqliteConfig
        where TUser : IHasId<TId>
    {
        
        protected List<TDtoBuilder> DtoBuilders = null!;
        protected KeyValueProviderMockBuilder MockKeyValueProvider = null!;

        public override void Setup()
        {
            base.Setup();

            DtoBuilders = new List<TDtoBuilder>();
            MockKeyValueProvider = new KeyValueProviderMockBuilder();
        }


        protected override TSut ConstructSut()
        {
            Services.TryAddTransient<TSut>();
            var serviceProvider = Services.BuildServiceProvider();
            var sut = serviceProvider.GetRequiredService<TSut>();

            Task.Run(async () =>
            {
                var dtoBatch = DtoBatch<TDto, TId>.Create(DtoBuilders.Select(x => x.Object).ToArray(), 0);
                await sut.SaveSyncedDtosAsync(dtoBatch);
            }).Wait();

            return sut;
        }
    }
}