using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id).HasName("employees_pkey");

        builder.ToTable("employees");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ContactsId).HasColumnName("contacts_id");

        builder.HasOne(d => d.Contacts).WithMany(p => p.Employees)
            .HasForeignKey(d => d.ContactsId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("employees_contacts_id_fkey");

        builder
            .HasMany(e => e.Subjects)
            .WithMany(s => s.Employees)
            .UsingEntity<EmployeeSubject>
            (j => j
                .HasOne(p => p.Subject)
                .WithMany(s => s.EmployeeSubjects)
                .HasForeignKey(es => es.SubjectId),
                j => j
                    .HasOne(p => p.Employee)
                    .WithMany(e => e.EmployeeSubjects)
                    .HasForeignKey(fk => fk.EmployeeId),
                j =>
                {
                    j.ToTable("employee_subject");
                    j.Property(p => p.Id).HasColumnName("id");
                    j.Property(p => p.EmployeeId).HasColumnName("employee_id");
                    j.Property(p => p.SubjectId).HasColumnName("subject_id");
                });

    }
}