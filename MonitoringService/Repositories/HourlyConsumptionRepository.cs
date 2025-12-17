using Microsoft.EntityFrameworkCore;
using MonitoringService.Entities;

namespace MonitoringService.Repositories
{
    public class HourlyConsumptionRepository : IHourlyConsumptionRepository
    {
        private readonly MonitoringDbContext _context;

        public HourlyConsumptionRepository(MonitoringDbContext context)
        {
            _context = context;
        }

        public async Task<HourlyConsumption?> FindAsync(Guid deviceId, DateOnly date, int hour)
        {
            return await _context.HourlyConsumptions
                .FirstOrDefaultAsync(h =>
                    h.DeviceId == deviceId &&
                    h.Date == date &&
                    h.Hour == hour);
        }

        public async Task<List<HourlyConsumption>> FindDailyAsync(Guid deviceId, DateOnly date)
        {
            return await _context.HourlyConsumptions
                .Where(h => h.DeviceId == deviceId && h.Date == date)
                .OrderBy(h => h.Hour)
                .ToListAsync();
        }

        public async Task SaveAsync(HourlyConsumption entity)
        {
            _context.HourlyConsumptions.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
