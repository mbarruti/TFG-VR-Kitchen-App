using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string GetSavePath()
    {
        string fileName = "saveData.json";
#if UNITY_ANDROID && !UNITY_EDITOR
        // Path for portable VR headsets (e.g., Oculus Quest)
        return Path.Combine(Application.persistentDataPath, fileName);
#else
        // Path for PC
        return Path.Combine(Application.dataPath, fileName);
#endif
    }

    public void SaveData(object data)
    {
        string json = JsonUtility.ToJson(data, true);
        string path = GetSavePath();
        File.WriteAllText(path, json);
        Debug.Log("Data saved to: " + path);
    }

    public T LoadData<T>()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            T data = JsonUtility.FromJson<T>(json);
            Debug.Log("Data loaded from: " + path);
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found at: " + path);
            return default;
        }
    }
}

