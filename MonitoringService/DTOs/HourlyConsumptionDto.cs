namespace MonitoringService.DTOs
{
    public class HourlyConsumptionDto
    {
        public int Hour { get; set; }        // 0..23
        public double EnergyKwh { get; set; }
    }
}
