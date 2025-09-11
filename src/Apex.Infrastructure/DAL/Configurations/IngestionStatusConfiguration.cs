using Apex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apex.Infrastructure.DAL.Configurations;

public class IngestionStatusConfiguration : IEntityTypeConfiguration<IngestionStatus>
{
    public void Configure(EntityTypeBuilder<IngestionStatus> builder)
    {
        builder.ToTable("ingestion_statuses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.DataType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.ErrorMessage)
            .HasColumnType("text")
            .IsRequired(false);

        builder.HasOne(i => i.Session)
            .WithMany(s => s.IngestionStatuses)
            .HasForeignKey(i => i.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}