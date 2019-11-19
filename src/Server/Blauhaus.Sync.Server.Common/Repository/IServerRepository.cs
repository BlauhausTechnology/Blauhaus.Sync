using Blauhaus.Sync.Common.Entity;
using Blauhaus.Sync.Common.Repository;

namespace Blauhaus.Sync.Server.Common.Repository
{
    public interface IServerRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        
    }
}