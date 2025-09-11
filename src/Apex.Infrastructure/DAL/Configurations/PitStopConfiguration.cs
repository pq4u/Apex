using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Infrastructure.DAL.Configurations;

public class PitStopConfiguration : IEntityTypeConfiguration<PitStop>
{
    public void Configure(EntityTypeBuilder<PitStop> builder)
    {
        builder.ToTable("pit_stops");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.LapNumber)
            .IsRequired();

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.PitDuration)
            .HasPrecision(5, 3);

        builder.HasOne(p => p.Session)
            .WithMany(s => s.PitStops)
            .HasForeignKey(p => p.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Driver)
            .WithMany(d => d.PitStops)
            .HasForeignKey(p => p.DriverId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
