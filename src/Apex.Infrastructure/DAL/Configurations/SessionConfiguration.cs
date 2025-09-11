using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Infrastructure.DAL.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Key)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Name)
            .HasMaxLength(100);

        builder.Property(e => e.GmtOffset)
            .HasMaxLength(10);

        builder.Property(e => e.Status)
            .HasMaxLength(20);

        builder.HasOne(s => s.Meeting)
            .WithMany(m => m.Sessions)
            .HasForeignKey(s => s.MeetingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}