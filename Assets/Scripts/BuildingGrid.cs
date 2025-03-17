using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);

    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;
    private bool _isDeleting = false;

    private string savePath;
    public Building[] buildingPrefabs; // Добавь массив префабов зданий в инспекторе

    [System.Serializable]
    public class BuildingSaveData
    {
        public List<BuildingData> buildings = new List<BuildingData>();
    }

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;

        savePath = Path.Combine(Application.persistentDataPath, "buildings.json");
        LoadBuildings();
    }

    private void Update()
    {


        if (_isDeleting && Input.GetMouseButtonDown(0))
        {
            TryDeleteBuilding();
        }

        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x)
                    available = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y)
                    available = false;

                if (available && IsPlaceTaken(x, y))
                    available = false;

                flyingBuilding.transform.position = new Vector3(x, 0, y);
                flyingBuilding.SetTransparent(available);

                if (available && Input.GetMouseButtonDown(0))
                    PlaceFlayingBuilding(x, y);
            }
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null)
                    return true;
            }
        }
        return false;
    }

    private void PlaceFlayingBuilding(int placeX, int placeY)
    {
        flyingBuilding.SetNormal();

        // Ставим здание в grid
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }

        flyingBuilding = null;

        SaveBuildings(); // Сохранение после размещения
    }


    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
            Destroy(flyingBuilding.gameObject);

        flyingBuilding = Instantiate(buildingPrefab); //Создаём копию из префаба
        flyingBuilding.name = buildingPrefab.name; //Сбрасываем имя (без Clone)
    }

    private void TryDeleteBuilding()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Building building = hit.collider.GetComponent<Building>();
            if (building != null)
            {
                RemoveBuilding(building);
            }
        }
    }

    private void RemoveBuilding(Building building)
    {
        Vector3 pos = building.transform.position;
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.z);

        for (int i = 0; i < building.Size.x; i++)
        {
            for (int j = 0; j < building.Size.y; j++)
            {
                grid[x + i, y + j] = null;
            }
        }
        Destroy(building.gameObject);

        SaveBuildings();
    }

    public void StartDeletingMode(bool isDeleting)
    {
        _isDeleting = isDeleting;
    }

    private void SaveBuildings()
    {
        List<BuildingData> buildingsData = new List<BuildingData>();
        HashSet<Building> savedBuildings = new HashSet<Building>(); // ✅ Уникальные здания

        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                Building building = grid[x, y];

                if (building != null && !savedBuildings.Contains(building)) // Проверяем дубликаты
                {
                    savedBuildings.Add(building); // ✅ Добавляем в HashSet, чтобы не повторялось

                    int prefabIndex = GetPrefabIndex(building);
                    if (prefabIndex == -1) continue; // Если индекс не найден — пропускаем

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



    private void LoadBuildings()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        BuildingSaveData data = JsonUtility.FromJson<BuildingSaveData>(json);

        foreach (var buildingData in data.buildings)
        {
            if (buildingData.prefabIndex < 0 || buildingData.prefabIndex >= buildingPrefabs.Length)
                continue;

            Building prefab = buildingPrefabs[buildingData.prefabIndex];
            Building newBuilding = Instantiate(prefab, new Vector3(buildingData.x, 0, buildingData.y), Quaternion.identity);

            // Правильно заполняем grid, чтобы не дублировать
            for (int x = 0; x < newBuilding.Size.x; x++)
            {
                for (int y = 0; y < newBuilding.Size.y; y++)
                {
                    grid[buildingData.x + x, buildingData.y + y] = newBuilding;
                }
            }
        }
    }


    private int GetPrefabIndex(Building building)
    {
        string buildingName = building.name.Replace("(Clone)", "").Trim(); // Убираем (Clone)
        Debug.Log($"{buildingPrefabs.Length}");
        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            string prefabName = buildingPrefabs[i].name;
            Debug.Log($"{buildingName}");
            Debug.Log($"Сравниваю: {buildingName} с {prefabName}"); // Лог для отладки

            if (buildingName == prefabName)
            {
                Debug.Log($"Найден индекс {i} для {buildingName}");
                return i;
            }
        }

        Debug.LogError($"Prefab index not found for {buildingName}");
        return -1;
    }

}
