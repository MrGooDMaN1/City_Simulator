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

    public static void SaveBuildings(Building[,] grid, Building[] buildingPrefabs, Vector2Int gridSize)
    {
        List<BuildingData> buildingsData = new List<BuildingData>();
        HashSet<Building> savedBuildings = new HashSet<Building>();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Building building = grid[x, y];

                if (building != null && !savedBuildings.Contains(building))
                {
                    savedBuildings.Add(building);

                    int prefabIndex = GetPrefabIndex(building, buildingPrefabs);
                    if (prefabIndex == -1) continue;

                    buildingsData.Add(new BuildingData
                    {
                        x = x,
                        y = y,
                        prefabIndex = prefabIndex
                    });
                }
            }
        }

        string json = JsonUtility.ToJson(new BuildingSaveData { buildings = buildingsData }, true);
        File.WriteAllText(savePath, json);
    }

    public static void LoadBuildings(Building[,] grid, Building[] buildingPrefabs, Vector2Int gridSize)
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        BuildingSaveData data = JsonUtility.FromJson<BuildingSaveData>(json);

        foreach (var buildingData in data.buildings)
        {
            if (buildingData.prefabIndex < 0 || buildingData.prefabIndex >= buildingPrefabs.Length)
                continue;

            Building prefab = buildingPrefabs[buildingData.prefabIndex];
            Building newBuilding = Object.Instantiate(prefab, new Vector3(buildingData.x, 0, buildingData.y), Quaternion.identity);

            for (int x = 0; x < newBuilding.Size.x; x++)
            {
                for (int y = 0; y < newBuilding.Size.y; y++)
                {
                    grid[buildingData.x + x, buildingData.y + y] = newBuilding;
                }
            }
        }
    }

    private static int GetPrefabIndex(Building building, Building[] buildingPrefabs)
    {
        string buildingName = building.name.Replace("(Clone)", "").Trim();
        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            if (buildingName == buildingPrefabs[i].name)
            {
                return i;
            }
        }
        return -1;
    }
}