using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Domain.Abstractions.DtoHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.Server.EFCore.Extensions;
using Blauhaus.Domain.Server.Entities;
using Blauhaus.Orleans.Abstractions.Resolver;
using Blauhaus.Orleans.EfCore.Grains;
using Blauhaus.Responses;
using Blauhaus.SignalR.Abstractions.Auth;
using Blauhaus.Sync.Abstractions.Common;
using Blauhaus.Sync.Server.Orleans.Abstractions;
using Blauhaus.Time.Abstractions;
using Microsoft.EntityFrameworkCore;
using EntityState = Blauhaus.Domain.Abstractions.Entities.EntityState;

namespace Blauhaus.Sync.Server.Orleans.Grains
{
    public abstract class BaseDtoSyncGrain<TDbContext, TDto, TEntity, TGrainResolver> : BaseDbGrain<TDbContext, TGrainResolver>, IDtoSyncGrain<TDto>
        where TDbContext : DbContext
        where TDto : IClientEntity<Guid>  
        where TGrainResolver : IGrainResolver
        where TEntity : BaseServerEntity, IDtoOwner<TDto>
    {
        
        protected readonly Dictionary<string, IConnectedUser> UserConnections = new();
        protected int BatchSize = 50;
        protected Dictionary<Guid, TDto> AllDtos = new();

        protected BaseDtoSyncGrain(
            Func<TDbContext> dbContextFactory, 
            IAnalyticsService analyticsService, 
            ITimeService timeService,
            TGrainResolver grainResolver) 
                : base(dbContextFactory, analyticsService, timeService, grainResolver)
        {
        }

        public Task SetBatchSizeAsync(int batchSize)
        {
            BatchSize = batchSize;
            return Task.CompletedTask;
        }

        public override async Task OnActivateAsync()
        {
            using var db = GetDbContext();

            var query = Include(db.Set<TEntity>().AsQueryable());
            query = await ModifySyncQueryAsync(query);
            var allEntities = await query.ToArrayAsync();
                
            foreach (var entity in allEntities)
            {
                AllDtos[entity.Id] =await entity.GetDtoAsync();
            }
        }

        public Task<Response<DtoBatch<TDto, Guid>>> HandleAsync(DtoSyncCommand command, IConnectedUser user)
        {
            var modifiedAfter = command.ModifiedAfterTicks;

            Func<TDto, bool> filter;

            if (command.IsFirstSync)
            {
                if (command.ModifiedAfterTicks > 0)
                {
                    filter = dto =>
                        dto.ModifiedAtTicks > modifiedAfter &&
                        dto.EntityState == EntityState.Active;
                }
                else
                {
                    filter = dto =>
                        dto.EntityState == EntityState.Active;
                }
            }
            else
            {
                filter = dto => dto.ModifiedAtTicks > modifiedAfter;
            } 

            //there is a problem when 2 entities have exactly the same modified and the first is the last one in a batch
            //the next batch will ask for modified after the previous one so entity 2 is exclude. Unlikely to happen but could be nasty
            //possible solutions - add any entities with exactly the same modified as the last one in the set?

            var totalCount = AllDtos.Values
                .Count(filter);

            var dtoBatch = AllDtos.Values
                .OrderBy(x => x.ModifiedAtTicks)
                .Where(filter)
                .Take(BatchSize).ToArray();

            return Response.SuccessTask(totalCount == 0 
                ? DtoBatch<TDto, Guid>.Empty() 
                : DtoBatch<TDto, Guid>.Create(dtoBatch, Math.Max(0, totalCount - dtoBatch.Length)));
        }

        protected virtual IQueryable<TEntity> Include(IQueryable<TEntity> query)
        {
            return query;
        }

        protected virtual Task<IQueryable<TEntity>> ModifySyncQueryAsync(IQueryable<TEntity> query)
        {
            return Task.FromResult(query);
        }

        
        public Task UpdateDtoAsync(TDto dto)
        {
            //todo update connected users
            AllDtos[dto.Id] = dto;
            return Task.CompletedTask;
        }


        
        public async Task ConnectUserAsync(IConnectedUser user)
        {
            if (!UserConnections.TryGetValue(user.UniqueId, out _))
            {
                UserConnections[user.UniqueId] = user;
                await HandleConnectedUserAsync(user);
            }
        }
        
        public async Task DisconnectUserAsync(IConnectedUser user)
        {
            if (UserConnections.TryGetValue(user.UniqueId, out _))
            {
                UserConnections.Remove(user.UniqueId);
                await HandleDisconnectedUserAsync(user);
            }
        }
        
            
        protected virtual Task HandleConnectedUserAsync(IConnectedUser user)
        {
            return Task.CompletedTask;
        }
        
        protected virtual Task HandleDisconnectedUserAsync(IConnectedUser user)
        {
            return Task.CompletedTask;
        }
    }
}