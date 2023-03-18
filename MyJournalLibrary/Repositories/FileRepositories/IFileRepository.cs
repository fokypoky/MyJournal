namespace MyJournalLibrary.Repositories.FileRepositories;

public interface IFileRepository<T>
{
    bool WriteFileToPath(T saveObject, string filePath);
    bool WriteFile(T saveObject);
    T ReadFileFromPath(string filePath);
    T ReadFile();
}