using System;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.DtoCaches;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Sync.Abstractions.Common;

namespace Blauhaus.Sync.Abstractions.Client
{
    public interface ISyncDtoCache<TDto, TId, in TUser> : IDtoCache<TDto, TId>
        where TDto : class, IClientEntity<TId> 
        where TId : IEquatable<TId>
        where TUser : IHasId<TId>
    {
        Task<long?> LoadLastModifiedTicksAsync(TUser? currentUser);
        Task SaveSyncedDtosAsync(DtoBatch<TDto, TId> dtoBatch);

    }
}