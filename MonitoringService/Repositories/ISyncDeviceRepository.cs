using MonitoringService.Entities;

namespace MonitoringService.Repositories
{
    public interface ISyncDeviceRepository
    {
        Task SaveAsync(SyncDevice device);
        Task<bool> ExistsAsync(Guid deviceId);
    }
}
