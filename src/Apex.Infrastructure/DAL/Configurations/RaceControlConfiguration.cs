using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Infrastructure.DAL.Configurations;

public class RaceControlConfiguration : IEntityTypeConfiguration<RaceControl>
{
    public void Configure(EntityTypeBuilder<RaceControl> builder)
    {
        builder.ToTable("race_controls");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.Category)
            .HasMaxLength(50);

        builder.Property(e => e.Flag)
            .HasMaxLength(20);

        builder.Property(e => e.Message)
            .HasColumnType("text");

        builder.HasOne(r => r.Session)
            .WithMany(s => s.RaceControls)
            .HasForeignKey(r => r.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Driver)
            .WithMany()
            .HasForeignKey(r => r.DriverId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}