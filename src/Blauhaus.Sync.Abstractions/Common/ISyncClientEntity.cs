using Blauhaus.Domain.Abstractions.Entities;

namespace Blauhaus.Sync.Abstractions.Common
{
    public interface ISyncClientEntity<out TId> : IClientEntity<TId>
    {
        SyncState SyncState { get; set; }
    }

}