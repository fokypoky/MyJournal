using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.EFModels;

namespace MyJournalLibrary.EFModelsConfiguration;

public class AuditoryConfiguration : IEntityTypeConfiguration<Auditory>
{
    public void Configure(EntityTypeBuilder<Auditory> builder)
    {
        builder.ToTable("Auditories");
        #region Auditory number
        builder
            .Property(a => a.Number)
            .HasColumnName("auditory_number");
        
        builder
            .Property(a => a.Number)
            .HasColumnType("varchar");
        
        builder
            .Property(a => a.Number)
            .HasMaxLength(10);
        
        builder
            .Property(a => a.Number)
            .IsRequired();

        #endregion
        #region Id
        builder
            .Property(a => a.Id)
            .HasColumnName("id");
        
        builder.HasKey(a => a.Id);
        #endregion
        #region References
        
        #endregion
    }
}