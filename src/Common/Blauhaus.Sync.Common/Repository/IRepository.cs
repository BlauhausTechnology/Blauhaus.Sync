using System;
using System.Threading.Tasks;
using Blauhaus.Sync.Common.Entity;

namespace Blauhaus.Sync.Common.Repository
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<TEntity> LoadAsync(Guid id);
    }
}