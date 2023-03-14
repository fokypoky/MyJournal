using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class EmployeeSubjectConfiguration : IEntityTypeConfiguration<EmployeeSubject>
{
    public void Configure(EntityTypeBuilder<EmployeeSubject> builder)
    {
        builder.HasKey(e => e.Id).HasName("employee_subject_pkey");

        builder.ToTable("employee_subject");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.EmployeeId).HasColumnName("employee_id");
        builder.Property(e => e.SubjectId).HasColumnName("subject_id");

        builder.HasOne(d => d.Employee).WithMany(p => p.EmployeeSubjects)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("employee_subject_employee_id_fkey");

        builder.HasOne(d => d.Subject).WithMany(p => p.EmployeeSubjects)
            .HasForeignKey(d => d.SubjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("employee_subject_subject_id_fkey");
    }
}