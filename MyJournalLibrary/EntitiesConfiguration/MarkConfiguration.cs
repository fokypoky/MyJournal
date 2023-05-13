using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class MarkConfiguration : IEntityTypeConfiguration<Mark>
{
    public void Configure(EntityTypeBuilder<Mark> builder)
    {
        builder.HasKey(e => e.Id).HasName("marks_pkey");

        builder.ToTable("marks");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.MarkValue).HasColumnName("mark");
        builder.Property(e => e.MarkDate)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("mark_date");
        builder.Property(e => e.StudentId).HasColumnName("student_id");
        builder.Property(e => e.TaskId).HasColumnName("task_id");
        builder.Property(e => e.TeacherId).HasColumnName("teacher_id");
        builder.Property(e => e.SubjectId).HasColumnName("subject_id");
        
        builder.HasOne(d => d.Student).WithMany(p => p.Marks)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("marks_student_id_fkey");

        builder.HasOne(d => d.Task).WithMany(p => p.Marks)
            .HasForeignKey(d => d.TaskId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("marks_task_id_fkey");

        builder.HasOne(d => d.Teacher).WithMany(p => p.Marks)
            .HasForeignKey(d => d.TeacherId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("marks_teacher_id_fkey");
        
        //builder.HasOne(e => e.)
        builder.HasOne(d => d.Subject)
            .WithMany(s => s.Marks)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_marks_subjects");
    }
}