using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public static class SaveSystem
{
    private static readonly string Path = $"{Application.persistentDataPath}/saves";
    
    public static void SaveToBinary<T>(string fileName, T data)
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }

        using var stream = new FileStream($"{Path}/{fileName}", FileMode.Create);
        byte[] buffer = ObjectToByteArray(data);
        stream.Write(buffer, 0, buffer.Length);
    }
    
    public static T LoadFromBinary<T>(string fileName)
    {
        if (!Directory.Exists($"{Path}") || !File.Exists($"{Path}/{fileName}"))
        {
            return default;
        }
        
        using var stream = new FileStream($"{Path}/{fileName}", FileMode.Open);
        var buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        return ByteArrayToObject<T>(buffer);
    }
    
    public static async Task SaveToBinaryAsync<T>(string fileName, T data)
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }

        using var stream = new FileStream($"{Path}/{fileName}", FileMode.Create);
        byte[] buffer = ObjectToByteArray(data);
        await stream.WriteAsync(buffer, 0, buffer.Length);
    }
    
    public static async Task<T> LoadFromBinaryAsync<T>(string fileName)
    {
        if (!Directory.Exists($"{Path}") || !File.Exists($"{Path}/{fileName}"))
        {
            return default;
        }
        
        using var stream = new FileStream($"{Path}/{fileName}", FileMode.Open);
        var buffer = new byte[stream.Length];
        await stream.ReadAsync(buffer, 0, buffer.Length);
        return ByteArrayToObject<T>(buffer);
    }

    public static void SaveToJson<T>(string fileName, T data)
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }

        using var outputFile = new StreamWriter($"{Path}/{fileName}");
        var json = JsonConvert.SerializeObject(data);
        outputFile.Write(json);
    }
    
    public static async Task SaveToJsonAsync<T>(string fileName, T data)
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }

        using var outputFile = new StreamWriter($"{Path}/{fileName}");
        var json = JsonConvert.SerializeObject(data);
        await outputFile.WriteAsync(json);
    }

    public static T LoadFromJson<T>(string fileName)
    {
        if (!Directory.Exists($"{Path}") || !File.Exists($"{Path}/{fileName}"))
        {
            return default;
        }

        using var inputFile = new StreamReader($"{Path}/{fileName}");
        var json = inputFile.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(json);
    }
    
    public static async Task<T> LoadFromJsonAsync<T>(string fileName)
    {
        if (!Directory.Exists($"{Path}") || !File.Exists($"{Path}/{fileName}"))
        {
            return default;
        }

        using var inputFile = new StreamReader($"{Path}/{fileName}");
        var json = await inputFile.ReadToEndAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }
    
    private static byte[] ObjectToByteArray(System.Object obj)
    {
        var binaryFormatter = new BinaryFormatter();
        using var stream = new MemoryStream();
        binaryFormatter.Serialize(stream, obj);
        return stream.ToArray();
    }

    private static T ByteArrayToObject<T>(byte[] arr)
    {
        var binaryFormatter = new BinaryFormatter();
        using var stream = new MemoryStream();
        stream.Write(arr, 0, arr.Length);
        stream.Position = 0;
        return (T) binaryFormatter.Deserialize(stream);
    }
}