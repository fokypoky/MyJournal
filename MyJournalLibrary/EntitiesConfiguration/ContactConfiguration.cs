using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.EntitiesConfiguration;
public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(e => e.Id).HasName("contacts_pkey");

        builder.ToTable("contacts");

        builder.HasIndex(e => e.Email, "contacts_email_key").IsUnique();

        builder.HasIndex(e => e.PhoneNumber, "contacts_phone_number_key").IsUnique();

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .HasColumnName("email");
        builder.Property(e => e.Midname)
            .HasMaxLength(50)
            .HasColumnName("midname");
        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .HasColumnName("name");
        builder.Property(e => e.Password)
            .HasMaxLength(20)
            .HasColumnName("password");
        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(15)
            .HasColumnName("phone_number");
        builder.Property(e => e.Sex)
            .HasMaxLength(1)
            .HasColumnName("sex");
        builder.Property(e => e.Surname)
            .HasMaxLength(50)
            .HasColumnName("surname");
        
    }
}