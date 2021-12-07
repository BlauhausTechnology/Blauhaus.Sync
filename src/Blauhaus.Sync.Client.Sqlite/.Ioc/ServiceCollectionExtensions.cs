using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Client.Sqlite.DtoCaches;
using Blauhaus.Sync.Client.Sqlite.Entities;
using Blauhaus.Sync.Client.SyncHandler;

namespace Blauhaus.Sync.Client.Sqlite.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteSync<TDto, TEntity, TSyncDtoCache, TId, TUser>(this IServiceCollection services)
            where TDto : class, IClientEntity<TId> 
            where TSyncDtoCache : SyncDtoCache<TDto, TEntity, TId, TUser>
            where TId : IEquatable<TId>
            where TEntity : BaseSyncClientEntity<TId>, new()
            where TUser : IHasId<TId>
        {
            
            services.AddSingleton<IDtoSyncHandler<TUser>, DtoSyncHandler<TDto, TId, TUser>>();
            services.TryAddSingleton<TSyncDtoCache>();
            services.TryAddSingleton<ISyncDtoCache<TDto, TId, TUser>>(sp => (ISyncDtoCache<TDto, TId, TUser>)sp.GetRequiredService<TSyncDtoCache>());
            
            return services;
        }
    }
}