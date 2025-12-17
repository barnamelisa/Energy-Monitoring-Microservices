using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringService.Entities
{
    [Table("sync_devices")]
    public class SyncDevice
    {
        [Key]
        [Column("device_id")]
        public Guid DeviceId { get; set; }

        [Column("user_id")]
        public string? UserId { get; set; }

        [Column("max_consumption")]
        public double? MaxConsumption { get; set; }

        [Column("name")]
        public string? Name { get; set; }
    }
}
