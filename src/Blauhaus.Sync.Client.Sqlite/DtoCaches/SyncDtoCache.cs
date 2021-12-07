using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.ClientActors.Actors;
using Blauhaus.ClientDatabase.Sqlite.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.Abstractions.Errors;
using Blauhaus.Errors;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Abstractions.Common;
using Blauhaus.Sync.Client.Sqlite.Entities;
using Newtonsoft.Json;
using SQLite;

namespace Blauhaus.Sync.Client.Sqlite.DtoCaches
{
    public class SyncDtoCache<TDto, TEntity, TId, TUser> : BaseActor, ISyncDtoCache<TDto, TId, TUser> 
        where TDto : class, IClientEntity<TId>
        where TEntity : BaseSyncClientEntity<TId>, new()
        where TId : IEquatable<TId>
        where TUser : IHasId<TId>
    {
        protected readonly IAnalyticsService AnalyticsService;
        protected readonly ISqliteDatabaseService SqliteDatabaseService;

        public SyncDtoCache(
            IAnalyticsService analyticsService,
            ISqliteDatabaseService sqliteDatabaseService)
        {
            AnalyticsService = analyticsService;
            SqliteDatabaseService = sqliteDatabaseService;
        }

        public Task<IDisposable> SubscribeAsync(Func<TDto, Task> handler, Func<TDto, bool>? filter = null)
        {
            return Task.FromResult(AddSubscriber(handler, filter));
        }

        public Task<long?> LoadLastModifiedTicksAsync(TUser? currentUser)
        {
            return InvokeAsync<long?>(async () =>
            {
                var query = SqliteDatabaseService.AsyncConnection.Table<TEntity>()
                    .Where(x => x.SyncState == SyncState.InSync);

                if (currentUser != null)
                {
                    var modifiedQuery = ApplyAdditionalFilters(query, currentUser);
                    if (modifiedQuery == null)
                    {
                        return null;
                    }

                    query = modifiedQuery;
                }

                query = query.OrderByDescending(x => x.ModifiedAtTicks)
                    .Take(1);

                var entity = await query.FirstOrDefaultAsync();

                return entity?.ModifiedAtTicks ?? 0; 
            });
        }
        
        protected virtual AsyncTableQuery<TEntity>? ApplyAdditionalFilters(AsyncTableQuery<TEntity> query, TUser currentUser)
        {
            return query;
        }

        public Task SaveSyncedDtosAsync(DtoBatch<TDto, TId> dtoBatch)
        {
            return InvokeAsync(async () =>
            {
                //todo cache BatchLastModifiedTicks?

                foreach (var dto in dtoBatch.Dtos)
                {
                    var entity = await PopulateEntityAsync(dto);
                    entity.SyncState = SyncState.InSync;

                    await SqliteDatabaseService.AsyncConnection
                        .InsertOrReplaceAsync(entity);

                    AnalyticsService.Debug($"{typeof(TDto).Name} saved as {typeof(TEntity).Name}");

                    await UpdateSubscribersAsync(dto);
                }
            });
        }
        

        public Task HandleAsync(TDto dto)
        {
            return InvokeAsync(async () =>
            {
                await SqliteDatabaseService.AsyncConnection
                    .InsertOrReplaceAsync(await PopulateEntityAsync(dto));

                AnalyticsService.Debug($"{typeof(TDto).Name} saved as {typeof(TEntity).Name}");

                await UpdateSubscribersAsync(dto);
            });
        }

        public Task<TDto> GetOneAsync(TId id)
        {
            return InvokeAsync(async () =>
            {
                var dto = await LoadOneAsync(id);

                if (dto == null)
                {
                    throw new ErrorException(DomainErrors.NotFound<TDto>());
                }

                return dto;
            });
        }

        public Task<TDto?> TryGetOneAsync(TId id)
        {
            return InvokeAsync(async ()=> await LoadOneAsync(id));
        }

        public Task<IReadOnlyList<TDto>> GetAllAsync()
        {
            return InvokeAsync(async () => await LoadManyAsync(x => true));
        }

        public Task DeleteOneAsync(TId id)
        {
            return InvokeAsync(async () =>
            {
                await SqliteDatabaseService.AsyncConnection.Table<TEntity>()
                    .DeleteAsync(x => x.Id.Equals(id));
            });
        }

        public Task DeleteAllAsync()
        {
            return InvokeAsync(async () =>
            {
                await SqliteDatabaseService.AsyncConnection.Table<TEntity>()
                    .DeleteAsync(x => true);
            });
        }


        protected async Task<IReadOnlyList<TDto>> LoadManyAsync(Expression<Func<TEntity, bool>>? search = null)
        {
            var entities = search == null 
                ? await SqliteDatabaseService.AsyncConnection.Table<TEntity>().ToListAsync()
                : await SqliteDatabaseService.AsyncConnection.Table<TEntity>().Where(search).ToListAsync();

            var dtos = new TDto[entities.Count];
            for (var i = 0; i < dtos.Length; i++)
            {
                dtos[i] = await PopulateDtoAsync(entities[i]);
            }

            return dtos;
        }

        protected async Task<IReadOnlyList<TId>> LoadManyIdsAsync(Expression<Func<TEntity, bool>>? search = null)
        {
            var entities = search == null 
                ? await SqliteDatabaseService.AsyncConnection.Table<TEntity>().ToListAsync()
                : await SqliteDatabaseService.AsyncConnection.Table<TEntity>().Where(search).ToListAsync();

            return entities.Select(x => x.Id).ToArray();
        }
        
        protected async Task<TDto?> LoadOneAsync(TId id)
        {
            var entity = await SqliteDatabaseService.AsyncConnection.Table<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (entity == null)
            {
                return null;
            }

            var dto = await PopulateDtoAsync(entity);
            return dto;
        }
         
        protected virtual Task<TEntity> PopulateEntityAsync(TDto dto)
        {
            return Task.FromResult(new TEntity
            {
                Id = dto.Id,
                EntityState = dto.EntityState,
                ModifiedAtTicks = dto.ModifiedAtTicks,
                SerializedDto = JsonConvert.SerializeObject(dto),
            }); 
        }

        protected virtual Task<TDto> PopulateDtoAsync(TEntity entity)
        {
            var dto = JsonConvert.DeserializeObject<TDto>(entity.SerializedDto);
            if (dto == null)
            {
                throw new InvalidOperationException($"Failed to deserialize {typeof(TDto).Name} from {typeof(TEntity).Name}");
            }

            return Task.FromResult(dto);
        }
    }
}