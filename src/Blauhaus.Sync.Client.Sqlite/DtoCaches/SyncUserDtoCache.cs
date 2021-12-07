//using System;
//using System.Threading.Tasks;
//using Blauhaus.Analytics.Abstractions.Service;
//using Blauhaus.ClientDatabase.Sqlite.Service;
//using Blauhaus.Common.Abstractions;
//using Blauhaus.Domain.Abstractions.Entities;
//using Blauhaus.Sync.Client.Sqlite.Entities;
//using SQLite;

//namespace Blauhaus.Sync.Client.Sqlite.DtoCaches
//{
//    public class SyncUserDtoCache<TDto, TEntity, TId> : SyncDtoCache<TDto, TEntity, TId> 
//        where TDto : class, IClientEntity<TId>, IHasUserId 
//        where TEntity : BaseSyncClientUserEntity<TId>, new() 
//        where TId : IEquatable<TId>
//    {
//        public SyncUserDtoCache(
//            IAnalyticsService analyticsService, 
//            ISqliteDatabaseService sqliteDatabaseService) 
//                : base(analyticsService, sqliteDatabaseService)
//        {
//        }

//        protected override AsyncTableQuery<TEntity>? ApplyAdditionalFilters(AsyncTableQuery<TEntity> query, IKeyValueProvider settingsProvider)
//        {
//            var userIdString = settingsProvider.TryGetValue("UserId");
//            if (userIdString != null && Guid.TryParse(userIdString, out var userId))
//            {
//                return query.Where(x => x.UserId == userId);
//            }

//            return null;
//        }

//        protected override async Task<TEntity> PopulateEntityAsync(TDto dto)
//        {
//            var entity = await base.PopulateEntityAsync(dto);
//            entity.UserId = dto.UserId;
//            return entity;
//        }
//    }
//}