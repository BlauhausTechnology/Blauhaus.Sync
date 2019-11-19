using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Blauhaus.Sync.Client.Common.Repository;
using Blauhaus.Sync.Common.Entity;
// ReSharper disable All

namespace Blauhaus.Sync.Client.Repository.SQLite.Repository
{
    public abstract class BaseSQLiteRepository<TEntity> : IClientRepository<TEntity> where TEntity : IEntity
    {
        public Task<TEntity> LoadAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}