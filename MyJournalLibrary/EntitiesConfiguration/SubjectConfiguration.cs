using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(e => e.Id).HasName("subjects_pkey");

        builder.ToTable("subjects");

        builder.HasIndex(e => e.SubjectTitle, "subjects_subject_title_key").IsUnique();

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.SubjectTitle)
            .HasMaxLength(50)
            .HasColumnName("subject_title");
    }
}