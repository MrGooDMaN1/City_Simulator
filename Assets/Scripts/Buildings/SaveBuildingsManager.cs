using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBuildingsManager : MonoBehaviour
{
    public static void SaveBuildings(Building[,] grid, Building[] buildingPrefabs, Vector2Int grid_size)
    {
        List<BuildingData> buildingsData = new List<BuildingData>();
        HashSet<Building> savedBuildings = new HashSet<Building>();

        for (int x = 0; x < grid_size.x; x++)
        {
            for (int y = 0; y < grid_size.y; y++)
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
                        _prefabIndex = prefabIndex
                    });
                }
            }
        }

        SaveManager.SaveBuildings(buildingsData);
    }

    public static void LoadBuildings(Building[,] grid, Building[] buildingPrefabs, Vector2Int grid_size)
    {
        List<BuildingData> buildingsData = SaveManager.LoadBuildings();

        foreach (var buildingData in buildingsData)
        {
            if (buildingData._prefabIndex < 0 || buildingData._prefabIndex >= buildingPrefabs.Length)
                continue;

            Building prefab = buildingPrefabs[buildingData._prefabIndex];
            Building newBuilding = Instantiate(prefab, new Vector3(buildingData.x, 0, buildingData.y), Quaternion.identity);

            for (int x = 0; x < newBuilding._size.x; x++)
            {
                for (int y = 0; y < newBuilding._size.y; y++)
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
