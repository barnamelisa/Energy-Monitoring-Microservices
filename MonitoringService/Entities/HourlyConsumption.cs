using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringService.Entities
{
    [Table("hourly_consumption")]
    public class HourlyConsumption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("device_id")]
        public Guid DeviceId { get; set; }

        [Required]
        [Column("consumption_date")]
        public DateOnly Date { get; set; }

        [Required]
        [Column("consumption_hour")]
        public int Hour { get; set; } // 0..23

        [Required]
        [Column("energy_kwh")]
        public double EnergyKwh { get; set; }

        public void AddEnergy(double delta)
        {
            EnergyKwh += delta;
        }
    }
}
