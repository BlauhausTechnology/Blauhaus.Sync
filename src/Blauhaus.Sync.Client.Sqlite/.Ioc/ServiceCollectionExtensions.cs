using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Client.Sqlite.DtoCaches;
using Blauhaus.Sync.Client.Sqlite.Entities;
using Blauhaus.Sync.Client.SyncHandler;

namespace Blauhaus.Sync.Client.Sqlite.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteSync<TDto, TEntity, TSyncDtoCache, TId>(this IServiceCollection services)
            where TDto : class, IClientEntity<TId> 
            where TSyncDtoCache : SyncDtoCache<TDto, TEntity, TId>
            where TId : IEquatable<TId>
            where TEntity : BaseSyncClientEntity<TId>, new()
        {
            
            services.AddSingleton<IDtoSyncHandler, DtoSyncHandler<TDto, TId>>();
            services.TryAddSingleton<TSyncDtoCache>();
            services.TryAddSingleton<ISyncDtoCache<TDto, TId>>(sp => (ISyncDtoCache<TDto, TId>)sp.GetRequiredService<TSyncDtoCache>());
            
            return services;
        }
    }
}