using System;
using System.Threading.Tasks;
using Blauhaus.Sync.Common.Entity;
using Blauhaus.Sync.Common.Repository;
using Blauhaus.Sync.Server.Common.Repository;

namespace Blauhaus.Sync.Server.Repository.Dapper.Repository
{
    public abstract class BaseDapperRepository<TEntity> : IServerRepository<TEntity> where TEntity : IEntity
    {
        public Task<TEntity> LoadAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}