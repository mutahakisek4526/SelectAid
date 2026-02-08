using System.IO;
using System.Text.Json;

namespace SelectAid.Persistence;

public class JsonStore
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public T Load<T>(string path, T fallback) where T : class
    {
        try
        {
            if (!File.Exists(path))
            {
                Save(path, fallback);
                return fallback;
            }
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json, _options) ?? fallback;
        }
        catch
        {
            Save(path, fallback);
            return fallback;
        }
    }

    public void Save<T>(string path, T data)
    {
        AppPaths.Ensure();
        var json = JsonSerializer.Serialize(data, _options);
        File.WriteAllText(path, json);
    }
}
