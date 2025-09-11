using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Infrastructure.DAL.Configurations;

public class SessionDriverConfiguration : IEntityTypeConfiguration<SessionDriver>
{
    public void Configure(EntityTypeBuilder<SessionDriver> builder)
    {
        builder.ToTable("session_drivers");

        builder.HasKey(sd => new { sd.SessionId, sd.DriverId });

        builder.HasOne(sd => sd.Session)
            .WithMany(s => s.SessionDrivers)
            .HasForeignKey(sd => sd.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sd => sd.Driver)
            .WithMany(d => d.SessionDrivers)
            .HasForeignKey(sd => sd.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sd => sd.Team)
            .WithMany()
            .HasForeignKey(sd => sd.TeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}