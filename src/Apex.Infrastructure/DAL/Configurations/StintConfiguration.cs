using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Infrastructure.DAL.Configurations;

public class StintConfiguration : IEntityTypeConfiguration<Stint>
{
    public void Configure(EntityTypeBuilder<Stint> builder)
    {
        builder.ToTable("stints");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.StintNumber)
            .IsRequired();

        builder.Property(e => e.LapStart)
            .IsRequired();

        builder.Property(e => e.LapEnd)
            .IsRequired();

        builder.Property(e => e.Compound)
            .HasMaxLength(20);

        builder.HasOne(s => s.Session)
            .WithMany(sess => sess.Stints)
            .HasForeignKey(s => s.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Driver)
            .WithMany(d => d.Stints)
            .HasForeignKey(s => s.DriverId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}