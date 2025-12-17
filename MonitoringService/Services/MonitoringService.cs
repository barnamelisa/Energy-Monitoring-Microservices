using MonitoringService.DTOs;
using MonitoringService.Repositories;

namespace MonitoringService.Services
{
    public class MonitoringService
    {
        private readonly IHourlyConsumptionRepository _hourlyRepo;

        public MonitoringService(IHourlyConsumptionRepository hourlyRepo)
        {
            _hourlyRepo = hourlyRepo;
        }

        /// <summary>
        /// Returns 24 hourly points (0..23) for a given device and date.
        /// Missing hours are filled with 0.
        /// </summary>
        public async Task<List<HourlyConsumptionDto>> GetDailyConsumptionAsync(
            Guid deviceId,
            DateOnly date)
        {
            var list = await _hourlyRepo.FindDailyAsync(deviceId, date);

            var energyByHour = list.ToDictionary(
                h => h.Hour,
                h => h.EnergyKwh
            );

            var result = new List<HourlyConsumptionDto>();

            for (int hour = 0; hour < 24; hour++)
            {
                result.Add(new HourlyConsumptionDto
                {
                    Hour = hour,
                    EnergyKwh = energyByHour.GetValueOrDefault(hour, 0.0)
                });
            }

            return result;
        }
    }
}
