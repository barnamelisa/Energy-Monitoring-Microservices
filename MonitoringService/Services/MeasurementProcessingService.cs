using MonitoringService.DTOs;
using MonitoringService.Entities;
using MonitoringService.Repositories;

namespace MonitoringService.Services
{
    public class MeasurementProcessingService
    {
        private readonly IHourlyConsumptionRepository _hourlyRepo;

        public MeasurementProcessingService(IHourlyConsumptionRepository hourlyRepo)
        {
            _hourlyRepo = hourlyRepo;
        }

        /// <summary>
        /// Processes a device measurement received from RabbitMQ:
        /// - extracts date and hour
        /// - aggregates hourly consumption (INSERT or UPDATE)
        /// </summary>
        public async Task ProcessMeasurementAsync(DeviceMeasurementMessage message)
        {
            if (message.DeviceId == Guid.Empty)
                return;

            if (message.Timestamp == null)
                return;

            if (message.MeasurementValue < 0)
                return;

            var dateTime = DateTimeOffset
                .FromUnixTimeMilliseconds(message.Timestamp)
                .ToLocalTime()
                .DateTime;

            var date = DateOnly.FromDateTime(dateTime);
            var hour = dateTime.Hour;

            var existing = await _hourlyRepo.FindAsync(
                message.DeviceId,
                date,
                hour
            );

            if (existing != null)
            {
                existing.AddEnergy(message.MeasurementValue);
                await _hourlyRepo.SaveAsync(existing);
            }
            else
            {
                var hourly = new HourlyConsumption
                {
                    DeviceId = message.DeviceId,
                    Date = date,
                    Hour = hour,
                    EnergyKwh = message.MeasurementValue
                };

                await _hourlyRepo.SaveAsync(hourly);
            }
        }
    }
}
