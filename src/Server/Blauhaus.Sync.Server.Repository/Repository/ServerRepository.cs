using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Blauhaus.Sync.Common.Entity;
using Blauhaus.Sync.Server.Common.Repository;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Blauhaus.Sync.Server.Repository.Repository
{
    public class ServerRepository<TEntity> : IServerRepository<TEntity> where TEntity : IEntity
    {
        private QueryFactory _db;

        public ServerRepository(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            var compiler = new SqliteCompiler();
            _db = new QueryFactory(connection, compiler);
        }

        public Task<TEntity> LoadAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var query = new Query(typeof(TEntity).Name).AsInsert(entity);
            return await query.<TEntity>();

        }
    }
}