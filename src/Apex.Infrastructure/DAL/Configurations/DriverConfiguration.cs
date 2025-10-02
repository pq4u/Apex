using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Infrastructure.DAL.Configurations;

internal class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("drivers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.DriverNumber)
            .IsRequired();

        builder.Property(e => e.BroadcastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FirstName)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(e => e.FullName)
            .HasMaxLength(100);

        builder.Property(e => e.NameAcronym)
            .HasMaxLength(3);

        builder.Property(e => e.CountryCode)
            .HasMaxLength(3);

        builder.HasIndex(e => e.DriverNumber);

        builder.Property(e => e.HeadshotUrl)
            .IsRequired(false);
    }
}
