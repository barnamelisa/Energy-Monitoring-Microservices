namespace MonitoringService.DTOs
{
    /// <summary>
    /// Message received from RabbitMQ (Device Data Simulator).
    /// Represents energy consumption over a 10-minute interval.
    /// </summary>
    public class DeviceMeasurementMessage
    {
        public Guid DeviceId { get; set; }

        /// <summary>
        /// Unix timestamp in milliseconds (UTC)
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Energy consumption for the interval (kWh)
        /// </summary>
        public double MeasurementValue { get; set; }
    }
}
