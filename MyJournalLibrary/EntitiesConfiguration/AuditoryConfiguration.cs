using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;


namespace MyJournalLibrary.EntitiesConfiguration;

public class AuditoryConfiguration : IEntityTypeConfiguration<Auditory>
{
    public void Configure(EntityTypeBuilder<Auditory> builder)
    {
        builder.HasKey(e => e.Id).HasName("auditories_pkey");

        builder.ToTable("auditories");
        builder.HasIndex(e => e.AuditoryNumber, "auditories_auditory_number_key").IsUnique();
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.AuditoryNumber)
            .HasMaxLength(10)
            .HasColumnName("auditory_number");
    }
}