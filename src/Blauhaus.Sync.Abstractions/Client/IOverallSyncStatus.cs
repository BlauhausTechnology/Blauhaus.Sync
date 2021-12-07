namespace Blauhaus.Sync.Abstractions.Client
{
    public interface IOverallSyncStatus
    {
        public int DownloadedDtoCount { get; }
        public int TotalDtoCount { get; }
        public float Progress { get; }
    }
}