using System;
using Blauhaus.Common.Abstractions;
using SQLite;

namespace Blauhaus.Sync.Client.Sqlite.Entities
{
    public abstract class BaseSyncClientUserEntity<TId> : BaseSyncClientEntity<TId>, IHasUserId
    {
        [Indexed]
        public Guid UserId { get; set; }
    }
}