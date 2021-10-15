using Blauhaus.Domain.Client.Sqlite.Entities;
using Blauhaus.Sync.Abstractions.Client;
using Blauhaus.Sync.Abstractions.Common;
using SQLite;

namespace Blauhaus.Sync.Client.Sqlite.Entities
{
    public abstract class BaseSyncClientEntity<TId> : ClientEntity<TId>, ISyncClientEntity<TId>, ISerializedDto
    {
        [Indexed]
        public SyncState SyncState { get; set; }

        public string SerializedDto { get; set; } = string.Empty;
    }
}