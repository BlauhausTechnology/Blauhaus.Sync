using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Responses;
using Blauhaus.Sync.Abstractions;
using Blauhaus.Sync.Abstractions.Client;

namespace Blauhaus.Sync.Client.SyncHandler
{
    public interface IDtoSyncHandler : IAsyncPublisher<DtoSyncStatus>
    {
        Task<Response> SyncDtoAsync(IKeyValueProvider? settingsProvider);
    }
}