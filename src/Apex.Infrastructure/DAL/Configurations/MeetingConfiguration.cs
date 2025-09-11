using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Infrastructure.DAL.Configurations;

public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.ToTable("meetings");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Key)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.OfficialName)
            .HasMaxLength(200);

        builder.Property(e => e.Location)
            .HasMaxLength(100);

        builder.Property(e => e.CountryKey)
            .HasMaxLength(3);

        builder.Property(e => e.CountryName)
            .HasMaxLength(50);

        builder.Property(e => e.CircuitKey)
            .HasMaxLength(20);

        builder.Property(e => e.CircuitShortName)
            .HasMaxLength(50);

        builder.Property(e => e.DateStart)
            .IsRequired();

        builder.Property(e => e.Year)
            .IsRequired();
    }
}