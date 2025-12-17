using MonitoringService.Entities;

namespace MonitoringService.Repositories
{
    public interface IHourlyConsumptionRepository
    {
        Task<HourlyConsumption?> FindAsync(Guid deviceId, DateOnly date, int hour);
        Task<List<HourlyConsumption>> FindDailyAsync(Guid deviceId, DateOnly date);
        Task SaveAsync(HourlyConsumption entity);
    }
}
