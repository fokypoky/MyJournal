using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.HasKey(e => e.Id).HasName("classes_pkey");

        builder.ToTable("classes");

        builder.HasIndex(e => e.ClassNumber, "classes_class_number_key").IsUnique();

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.AuditoryId).HasColumnName("auditory_id");
        builder.Property(e => e.ClassNumber)
            .HasMaxLength(5)
            .HasColumnName("class_number");
        builder.Property(e => e.LeaderId).HasColumnName("leader_id");

        builder.HasOne(d => d.Auditory).WithMany(p => p.Classes)
            .HasForeignKey(d => d.AuditoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("classes_auditory_id_fkey");

        builder.HasOne(d => d.Leader).WithMany(p => p.Classes)
            .HasForeignKey(d => d.LeaderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("classes_leader_id_fkey");

        builder
            .HasMany(c => c.Subjects)
            .WithMany(s => s.Classes)
            .UsingEntity<ClassSubject>
            (
                e => e
                    .HasOne(cs => cs.Subject)
                    .WithMany(s => s.ClassSubjects)
                    .HasForeignKey(cs => cs.SubjectId),
                e => e
                    .HasOne(cs => cs.Class)
                    .WithMany(c => c.ClassSubjects)
                    .HasForeignKey(fk => fk.ClassId),
                e =>
                {
                    e.ToTable("class_subject");
                    e.Property(p => p.Id).HasColumnName("id");
                    e.Property(p => p.SubjectId).HasColumnName("subject_id");
                    e.Property(p => p.ClassId).HasColumnName("class_id");
                }
            );
    }
}