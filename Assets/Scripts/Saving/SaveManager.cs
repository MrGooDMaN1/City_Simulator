using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string _savePath = Path.Combine(Application.persistentDataPath, "_buildings.json");

    [System.Serializable]
    public class BuildingSaveData
    {
        public List<BuildingData> _buildings = new List<BuildingData>();
    }

    public static void SaveBuildings(List<BuildingData> _buildingsData)
    {
        string json = JsonUtility.ToJson(new BuildingSaveData { _buildings = _buildingsData }, true);
        File.WriteAllText(_savePath, json);
    }

    public static List<BuildingData> LoadBuildings()
    {
        if (!File.Exists(_savePath)) return new List<BuildingData>();

        string json = File.ReadAllText(_savePath);
        return JsonUtility.FromJson<BuildingSaveData>(json)._buildings;
    }
}
