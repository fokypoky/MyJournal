using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournalLibrary.EntitiesConfiguration;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(e => e.Id).HasName("tasks_pkey");

        builder.ToTable("tasks");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ClassId).HasColumnName("class_id");
        builder.Property(e => e.SubjectId).HasColumnName("subject_id");
        builder.Property(e => e.TaskText)
            .HasMaxLength(200)
            .HasColumnName("task_text");
        builder.Property(e => e.TeacherId).IsRequired(false).HasColumnName("teacher_id");

        builder.Property(e => e.StartDate).HasColumnName("task_start_date");
        builder.Property(e => e.EndDate).HasColumnName("task_deadline_date");

        builder.HasOne(d => d.Class).WithMany(p => p.Tasks)
            .HasForeignKey(d => d.ClassId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tasks_class_id_fkey");

        builder.HasOne(d => d.Subject).WithMany(p => p.Tasks)
            .HasForeignKey(d => d.SubjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tasks_subject_id_fkey");

        builder.HasOne(d => d.Teacher).WithMany(p => p.Tasks)
            .HasForeignKey(d => d.TeacherId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tasks_teacher_id_fkey");
    }
}