using Microsoft.EntityFrameworkCore;
using MonitoringService.Entities;

namespace MonitoringService.Repositories
{
    public class SyncDeviceRepository : ISyncDeviceRepository
    {
        private readonly MonitoringDbContext _context;

        public SyncDeviceRepository(MonitoringDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(SyncDevice device)
        {
            if (!await ExistsAsync(device.DeviceId))
            {
                _context.SyncDevices.Add(device);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid deviceId)
        {
            return await _context.SyncDevices.AnyAsync(d => d.DeviceId == deviceId);
        }
    }
}
