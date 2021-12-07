//using Blauhaus.Domain.Abstractions.CommandHandlers;
//using Blauhaus.Domain.Abstractions.DtoHandlers;
//using Blauhaus.Domain.Abstractions.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Blauhaus.Analytics.Abstractions.Service;
//using Blauhaus.Common.Abstractions;
//using Blauhaus.Time.Abstractions;

//namespace Blauhaus.Sync.Server.EfCore.SyncHandlers
//{
//    public abstract class BaseUserDtoSyncCommandHandler<TDbContext, TEntity, TDto, TId, TUser> : BaseDtoSyncCommandHandler<TDbContext, TEntity, TDto, TId, TUser>
//        where TDto : IClientEntity<TId>, IHasUserId
//        where TId : IEquatable<TId>
//        where TEntity: class, IServerEntity, IDtoOwner<TDto>, IHasUserId
//        where TDbContext : DbContext
//        where TUser : IHasUserId, IHasId<Guid>
//    {
//        protected BaseUserDtoSyncCommandHandler(
//            IAnalyticsService analyticsService, 
//            ITimeService timeService, 
//            Func<TDbContext> dbContextFactory) 
//                : base(analyticsService, timeService, dbContextFactory)
//        {
//        }

//        protected override Task<IQueryable<TEntity>> ModifySyncQueryForUserAsync(IQueryable<TEntity> query, TUser user)
//        {
//            return Task.FromResult(query.Where(x => x.UserId == user.UserId));
//        }
//    }
//}