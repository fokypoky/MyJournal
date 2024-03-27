using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(e => e.Id).HasName("students_pkey");

        builder.ToTable("students");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ClassId).IsRequired(false).HasColumnName("class_id");
        builder.Property(e => e.ContactsId).HasColumnName("contacts_id");

        builder.HasOne(d => d.Class).WithMany(p => p.Students)
            .HasForeignKey(d => d.ClassId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("students_class_id_fkey");

        builder.HasOne(d => d.Contacts).WithMany(p => p.Students)
            .HasForeignKey(d => d.ContactsId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("students_contacts_id_fkey");

        builder
            .HasMany(s => s.Parents)
            .WithMany(p => p.Students)
            .UsingEntity<ParentStudent>
            (j => j
                    .HasOne(p => p.Parent)
                    .WithMany(p => p.ParentStudents)
                    .HasForeignKey(fk => fk.ParentId),
                j => j
                    .HasOne(p => p.Student)
                    .WithMany(p => p.ParentStudents)
                    .HasForeignKey(fk => fk.StudentId),
                j =>
                {
                    j.ToTable("parent_student");
                    j.Property(p => p.Id).HasColumnName("id");
                    j.Property(p => p.ParentId).HasColumnName("parent_id");
                    j.Property(p => p.StudentId).HasColumnName("student_id");
                }
            );
    }
}