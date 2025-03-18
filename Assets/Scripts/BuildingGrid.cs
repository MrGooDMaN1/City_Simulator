using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    public Building[] buildingPrefabs;

    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;
    private bool _isDeleting = false;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;

        SaveManager.LoadBuildings(grid, buildingPrefabs, GridSize);
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

        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }

        flyingBuilding = null;

        SaveManager.SaveBuildings(grid, buildingPrefabs, GridSize);
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
            Destroy(flyingBuilding.gameObject);

        flyingBuilding = Instantiate(buildingPrefab);
        flyingBuilding.name = buildingPrefab.name;
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

        SaveManager.SaveBuildings(grid, buildingPrefabs, GridSize);
    }

    public void StartDeletingMode(bool isDeleting)
    {
        _isDeleting = isDeleting;
    }
}