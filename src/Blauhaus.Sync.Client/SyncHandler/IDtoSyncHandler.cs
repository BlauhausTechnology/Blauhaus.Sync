using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Responses;
using Blauhaus.Sync.Abstractions;
using Blauhaus.Sync.Abstractions.Client;

namespace Blauhaus.Sync.Client.SyncHandler
{
    public interface IDtoSyncHandler<in TUser> : IAsyncPublisher<DtoSyncStatus>
    {
        Task<Response> SyncDtoAsync(TUser? currentUser);
    }
}