using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;

public class ParentConfiguration : IEntityTypeConfiguration<Parent>
{
    public void Configure(EntityTypeBuilder<Parent> builder)
    {
        builder.HasKey(e => e.Id).HasName("parents_pkey");

        builder.ToTable("parents");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ContactsId).HasColumnName("contacts_id");

        builder.HasOne(d => d.Contacts).WithMany(p => p.Parents)
            .HasForeignKey(d => d.ContactsId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("parents_contacts_id_fkey");
    }
}