using System.Collections.Generic;
using Blauhaus.Domain.Abstractions.Entities;

namespace Blauhaus.Sync.Abstractions
{
    public class SyncResult<TPayload>  
        where TPayload : IEntity
    {
        public List<TPayload> EntityBatch { get; set; } = new List<TPayload>();
        public long TotalActiveEntityCount { get; set; }
        public long EntitiesToDownloadCount { get; set; }
    }
}