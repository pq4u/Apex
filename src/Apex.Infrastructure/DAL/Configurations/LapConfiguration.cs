using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Infrastructure.DAL.Configurations;

public class LapConfiguration : IEntityTypeConfiguration<Lap>
{
    public void Configure(EntityTypeBuilder<Lap> builder)
    {
        builder.ToTable("laps");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.SegmentsJson)
            .HasColumnType("jsonb");

        builder.HasIndex(e => new { e.SessionId, e.DriverId, e.LapNumber })
            .IsUnique();

        builder.HasIndex(e => new { e.SessionId, e.DriverId });

        builder.HasOne(l => l.Session)
            .WithMany(s => s.Laps)
            .HasForeignKey(l => l.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Driver)
            .WithMany(d => d.Laps)
            .HasForeignKey(l => l.DriverId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}