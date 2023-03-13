using Microsoft.EntityFrameworkCore;

namespace MyJournal.Models;

public class ApplicationContext : DbContext
{

    public static string ConnectionString =
        "Host=localhost;Port=5432;Username=postgres;Password=toor;Database=MyJournalDB";
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}