using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "buildings.json");

    [System.Serializable]
    public class BuildingSaveData
    {
        public List<BuildingData> buildings = new List<BuildingData>();
    }

    public static void SaveBuildings(List<BuildingData> buildingsData)
    {
        string json = JsonUtility.ToJson(new BuildingSaveData { buildings = buildingsData }, true);
        File.WriteAllText(savePath, json);
    }

    public static List<BuildingData> LoadBuildings()
    {
        if (!File.Exists(savePath)) return new List<BuildingData>();

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<BuildingSaveData>(json).buildings;
    }
}
