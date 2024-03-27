using System.Diagnostics;
using System.Runtime.Serialization.Json;

namespace MyJournalLibrary.Repositories.FileRepositories;

public class JsonRepository<T> : IFileRepository<T>
{
    private string _filePath;
    private DataContractJsonSerializer _serializer;
    public JsonRepository(string filePath)
    {
        _filePath = filePath;
        _serializer = new DataContractJsonSerializer(typeof(T));
    }
    public JsonRepository()
    {
        _serializer = new DataContractJsonSerializer(typeof(T));
    }
    private bool IsFilePathEmpty(string path){
        
        if(String.IsNullOrEmpty(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool WriteFile(T saveObject) => WriteFileToPath(saveObject ,_filePath);
    public bool WriteFileToPath(T saveObject, string filePath){

        if(IsFilePathEmpty(filePath))
        {
            return false;
        }

        File.WriteAllText(filePath, "");

        using(var stream = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            try
            {
                _serializer.WriteObject(stream, saveObject);
                return true;
            }
            catch(Exception exception)
            {
                Debug.WriteLine(exception);
                return false;
            }
        }
    }
    public T ReadFileFromPath(string filePath)
    {

        if(IsFilePathEmpty(filePath))
        {
            return default(T);
        }
        if(!File.Exists(filePath))
        {
            return default(T);
        }

        using(var stream = new FileStream(filePath, FileMode.Open))
        {
            try
            {
                var _object = (T)_serializer.ReadObject(stream);
                return _object;
            }
            catch
            {
                return default(T);
            }
        }
    }
    public T ReadFile() => ReadFileFromPath(_filePath);
}