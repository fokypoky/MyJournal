using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.EFModels;

namespace MyJournalLibrary.EFModelsConfiguration;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.ToTable("classes");
        
        #region Class Number

        builder
            .Property(c => c.Number)
            .HasColumnName("class_number");

        builder
            .Property(c => c.Number)
            .HasColumnType("varchar");

        builder
            .Property(c => c.Number)
            .HasMaxLength(5);

        builder
            .Property(c => c.Number)
            .IsRequired();

        #endregion

        #region Leader id
        builder
            .Property(c => c.LeaderId)
            .HasColumnName("leader_id");
        builder
            .Property(c => c.LeaderId)
            .HasColumnType("int");

        #endregion

        #region Auditory id

        builder
            .Property(c => c.AuditoryId)
            .HasColumnName("auditory_id");
        builder
            .Property(c => c.AuditoryId)
            .HasColumnType("int");
        #endregion

        #region Students

        builder
            .HasMany(c => c.Students)
            .WithOne(s => s.Class)
            .HasForeignKey(s => s.ClassId);


        #endregion

        #region Timetable

        builder
            .HasMany(c => c.Timetable)
            .WithOne(t => t.Class)
            .HasForeignKey(t => t.ClassId);

        #endregion
    }
}