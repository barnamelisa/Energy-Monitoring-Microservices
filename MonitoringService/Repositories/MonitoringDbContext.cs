using Microsoft.EntityFrameworkCore;
using MonitoringService.Entities;

namespace MonitoringService.Repositories
{
    public class MonitoringDbContext : DbContext
    {
        public MonitoringDbContext(DbContextOptions<MonitoringDbContext> options)
            : base(options)
        {
        }

        public DbSet<HourlyConsumption> HourlyConsumptions => Set<HourlyConsumption>();
        public DbSet<SyncDevice> SyncDevices => Set<SyncDevice>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HourlyConsumption>()
                .HasIndex(h => new { h.DeviceId, h.Date, h.Hour })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
