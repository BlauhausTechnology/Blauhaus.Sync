using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.Client.Sqlite.Entities;
using SQLite;

namespace Blauhaus.Sync.Client.Sqlite
{
    public abstract class SyncClientEntity<TId> : ClientEntity<TId>, ISyncClientEntity<TId>
    {
        [Indexed]
        public SyncState SyncState { get; set; }

        public string SerializedDto { get; set; } = string.Empty;
    }
}