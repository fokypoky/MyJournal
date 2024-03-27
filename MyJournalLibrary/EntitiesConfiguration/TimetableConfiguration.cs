using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class TimetableConfiguration : IEntityTypeConfiguration<Timetable>
{
    public void Configure(EntityTypeBuilder<Timetable> builder)
    {
        builder.HasKey(e => e.Id).HasName("timetable_pkey");

        builder.ToTable("timetable");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.AuditoryId).IsRequired(false).HasColumnName("auditory_id");
        builder.Property(e => e.ClassId).HasColumnName("class_id");
        builder.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
        builder.Property(e => e.LessonTime).HasColumnName("lesson_time");
        builder.Property(e => e.SubjectId).HasColumnName("subject_id");
        builder.Property(e => e.TeacherId).IsRequired(false).HasColumnName("teacher_id");

        builder.HasOne(d => d.Auditory).WithMany(p => p.Timetables)
            .HasForeignKey(d => d.AuditoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("timetable_auditory_id_fkey");

        builder.HasOne(d => d.Class).WithMany(p => p.Timetables)
            .HasForeignKey(d => d.ClassId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("timetable_class_id_fkey");

        builder.HasOne(d => d.Subject).WithMany(p => p.Timetables)
            .HasForeignKey(d => d.SubjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("timetable_subject_id_fkey");

        builder.HasOne(d => d.Teacher).WithMany(p => p.Timetables)
            .HasForeignKey(d => d.TeacherId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("timetable_teacher_id_fkey");
    }
}