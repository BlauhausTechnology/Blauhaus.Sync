using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Responses;

namespace Blauhaus.Sync.Abstractions.Client
{
    public interface ISyncManager<in TUser> : IAsyncPublisher<IOverallSyncStatus>
    {
        Task<Response> SyncAllAsync(TUser? currentUser);
    }
}