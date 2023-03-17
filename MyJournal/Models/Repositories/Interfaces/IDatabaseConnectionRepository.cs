namespace MyJournal.Models.Repositories.Interfaces;

public interface IDatabaseConnectionRepository
{
    void Save();
    DatabaseConnection Load();
}