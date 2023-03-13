using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.EFModels;
using MyJournalLibrary.EFModelsConfiguration;

namespace MyJournal.Models;

public class ApplicationContext : DbContext
{
    public DbSet<Auditory> Auditories { get; set; }
    public DbSet<Class> Classes { get; set; }

    public static string ConnectionString =
        "Host=localhost;Port=5432;Username=postgres;Password=toor;Database=MyJournalDB";
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuditoryConfiguration());
        modelBuilder.ApplyConfiguration(new ClassConfiguration());
    }
}