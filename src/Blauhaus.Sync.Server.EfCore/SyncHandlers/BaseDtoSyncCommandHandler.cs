using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Domain.Abstractions.DtoHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Responses;
using Blauhaus.Sync.Abstractions.Common;
using Blauhaus.Time.Abstractions;
using Microsoft.EntityFrameworkCore;
using EntityState = Blauhaus.Domain.Abstractions.Entities.EntityState;

namespace Blauhaus.Sync.Server.EfCore.SyncHandlers
{
    public abstract class BaseDtoSyncCommandHandler<TDbContext, TEntity, TDto, TId, TUser> : IAuthenticatedCommandHandler<DtoBatch<TDto, TId>, DtoSyncCommand, TUser>
        where TDto : IClientEntity<TId> where TId : IEquatable<TId>
        where TEntity: class, IServerEntity, IDtoOwner<TDto>
        where TDbContext : DbContext
        
    {
        protected readonly IAnalyticsService AnalyticsService;
        protected readonly ITimeService TimeService;
        private readonly Func<TDbContext> _dbContextFactory;
        protected TDbContext GetDbContext() =>
            _dbContextFactory.Invoke();

        protected BaseDtoSyncCommandHandler(
            IAnalyticsService analyticsService,
            ITimeService timeService,
            Func<TDbContext> dbContextFactory)
        {
            AnalyticsService = analyticsService;
            TimeService = timeService;
            _dbContextFactory = dbContextFactory;
        }

        public int MaxBatchSize { get; set; } = 50;

        public async Task<Response<DtoBatch<TDto, TId>>> HandleAsync(DtoSyncCommand command, TUser user)
        {
            var modifiedAfter = new DateTime(command.ModifiedAfterTicks);

            Expression<Func<TEntity, bool>> filter;

            if (command.IsFirstSync)
            {
                if (command.ModifiedAfterTicks > 0)
                {
                    filter = entity =>
                        entity.ModifiedAt > modifiedAfter &&
                        entity.EntityState == EntityState.Active || entity.EntityState == EntityState.Archived;
                }
                else
                {
                    filter = entity =>
                        entity.EntityState == EntityState.Active || entity.EntityState == EntityState.Archived;
                }
            }
            else
            {
                filter = entity => entity.ModifiedAt > modifiedAfter;
            }

            using (var db = GetDbContext())
            {
                var query = Include(db.Set<TEntity>());

                query = await ModifySyncQueryForUserAsync(query, user);
                query = query.Where(filter);

                var totalCount = await query.CountAsync();
                if (totalCount == 0)
                {
                    return Response.Success(DtoBatch<TDto, TId>.Empty());
                }

                var entityBatch = await query.AsNoTracking()
                    .OrderBy(x => x.ModifiedAt)
                    .Take(MaxBatchSize)
                    .ToArrayAsync();

                var dtos = new TDto[entityBatch.Length];
                for (var i = 0; i < entityBatch.Length; i++)
                {
                    dtos[i] = await entityBatch[i].GetDtoAsync();
                }

                return Response.Success(DtoBatch<TDto, TId>.Create(dtos, Math.Max(0, totalCount - entityBatch.Length)));
            }

        }

        protected virtual IQueryable<TEntity> Include(IQueryable<TEntity> query)
        {
            return query;
        }

        protected virtual Task<IQueryable<TEntity>> ModifySyncQueryForUserAsync(IQueryable<TEntity> query, TUser user)
        {
            return Task.FromResult(query);
        }

    }
}