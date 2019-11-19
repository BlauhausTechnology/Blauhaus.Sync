using Blauhaus.Sync.Common.Entity;
using Blauhaus.Sync.Common.Repository;

namespace Blauhaus.Sync.Client.Common.Repository
{
    public interface IClientRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        
    }
}