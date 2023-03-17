using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class ParentStudentConfiguration : IEntityTypeConfiguration<ParentStudent>
{
    public void Configure(EntityTypeBuilder<ParentStudent> builder)
    {
        builder.HasKey(e => e.Id).HasName("parent_student_pkey");

        builder.ToTable("parent_student");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ParentId).HasColumnName("parent_id");
        builder.Property(e => e.StudentId).HasColumnName("student_id");

        builder.HasOne(d => d.Parent).WithMany(p => p.ParentStudents)
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("parent_student_parent_id_fkey");

        builder.HasOne(d => d.Student).WithMany(p => p.ParentStudents)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("parent_student_student_id_fkey");
    }
}