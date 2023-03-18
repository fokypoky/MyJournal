using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => e.Id).HasName("userroles_pkey");
        builder.ToTable("userroles");
        builder.HasIndex(e => e.Rolename, "userroles_rolename_key").IsUnique();
        builder.Property(e => e.Id).HasColumnName("id");
        
        builder.Property(e => e.Rolename)
            .HasMaxLength(16)
            .HasColumnName("rolename");
        builder
            .HasMany(ur => ur.Contacts)
            .WithOne(c => c.UserRole);
    }
}