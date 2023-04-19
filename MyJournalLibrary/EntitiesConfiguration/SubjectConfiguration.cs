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
        builder
            .HasMany(s => s.Employees)
            .WithMany(e => e.Subjects)
            .UsingEntity(e => e.ToTable("employee_subject"));
        builder
            .HasMany(s => s.Employees)
            .WithMany(e => e.Subjects)
            .UsingEntity<EmployeeSubject>
            (j => j
                    .HasOne(p => p.Employee)
                    .WithMany(p => p.EmployeeSubjects)
                    .HasForeignKey(fk => fk.EmployeeId),
                j => j
                    .HasOne(p => p.Subject)
                    .WithMany(p => p.EmployeeSubjects)
                    .HasForeignKey(fk => fk.SubjectId),
                j =>
                {
                    j.ToTable("employee_subject");
                    j.Property(p => p.Id).HasColumnName("id");
                    j.Property(p => p.EmployeeId).HasColumnName("employee_id");
                    j.Property(p => p.SubjectId).HasColumnName("subject_id");
                });
        builder
            .HasMany(s => s.Classes)
            .WithMany(c => c.Subjects)
            .UsingEntity<ClassSubject>
            (
                e => e
                    .HasOne(cs => cs.Class)
                    .WithMany(c => c.ClassSubjects)
                    .HasForeignKey(fk => fk.ClassId),
                e => e
                    .HasOne(cs => cs.Subject)
                    .WithMany(s => s.ClassSubjects)
                    .HasForeignKey(fk => fk.SubjectId),
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