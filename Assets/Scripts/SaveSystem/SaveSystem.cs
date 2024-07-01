using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveSystem : MonoBehaviour
{
    private string GetSavePath(string fileName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // Path for portable VR headsets (e.g., Oculus Quest)
        return Path.Combine(Application.persistentDataPath, fileName);
#else
        // Path for PC
        return Path.Combine(Application.dataPath, fileName);
#endif
    }

    public void SaveData(object data, string fileName)
    {
        string json = JsonConvert.SerializeObject(data);
        string path = GetSavePath(fileName);
        File.WriteAllText(path, JsonPrettify(json));
        Debug.Log("Data saved to: " + path);
    }

    public T LoadData<T>(string fileName)
    {
        string path = GetSavePath(fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            T data = JsonConvert.DeserializeObject<T>(json);
            Debug.Log("Data loaded from: " + path);
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found at: " + path);
            return default;
        }
    }

    public static string JsonPrettify(string json)
    {
        using (var stringReader = new StringReader(json))
        using (var stringWriter = new StringWriter())
        {
            var jsonReader = new JsonTextReader(stringReader);
            var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
            jsonWriter.WriteToken(jsonReader);
            return stringWriter.ToString();
        }
    }
}

