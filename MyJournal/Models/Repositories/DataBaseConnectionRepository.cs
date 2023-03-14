using System;
using System.IO;
using MyJournal.Models.Repositories.Interfaces;
using System.Runtime.Serialization.Json;

namespace MyJournal.Models.Repositories;

public class DataBaseConnectionRepository : IDatabaseConnectionRepository
{
    private DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(DatabaseConnection));
    private readonly string _filePath = "ConnectionSettings.json";
    public DatabaseConnection DatabaseConnection { get; set; }
    
    public DataBaseConnectionRepository(DatabaseConnection databaseConnection)
    {
        DatabaseConnection = databaseConnection;
    }

    public DataBaseConnectionRepository(string connectionString)
    {
       DatabaseConnection = new DatabaseConnection(connectionString);
    }
    public DataBaseConnectionRepository()
    {
        
    }
    public void Save()
    {
        File.WriteAllText(_filePath, "");
        using (var stream = new FileStream(_filePath,  FileMode.Truncate))
        {
            _serializer.WriteObject(stream, DatabaseConnection);
        }
    }
    
    public DatabaseConnection Load()
    {
        DatabaseConnection connection;
        if (!File.Exists(_filePath))
        {
            return null;
        }
        using (var stream = new FileStream(_filePath, FileMode.Open))
        {
            connection = (DatabaseConnection) _serializer.ReadObject(stream);
            DatabaseConnection = connection;
        }
        return connection;
    }
}