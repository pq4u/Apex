using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Apex.Infrastructure.DAL;

public class ApexDbContext : DbContext
{
    public ApexDbContext(DbContextOptions<ApexDbContext> options) : base(options) { }

    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<SessionDriver> SessionDrivers { get; set; }
    public DbSet<Lap> Laps { get; set; }
    public DbSet<Stint> Stints { get; set; }
    public DbSet<PitStop> PitStops { get; set; }
    public DbSet<RaceControl> RaceControls { get; set; }
    public DbSet<IngestionStatus> IngestionStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("domain");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApexDbContext).Assembly);

        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
        }
    }
}