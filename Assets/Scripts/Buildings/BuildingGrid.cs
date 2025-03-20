using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public Vector2Int _gridSize = new Vector2Int(10, 10);
    public Building[] _buildingPrefabs;
    public Building[,] _grid;

    private Building _flyingBuilding;
    private Camera _mainCamera;
    private bool _isPlacing = false;
    private bool _isDeleting = false;

    private void Awake()
    {
        _grid = new Building[_gridSize.x, _gridSize.y];
        _mainCamera = Camera.main;

        SaveBuildingsManager.LoadBuildings(_grid, _buildingPrefabs, _gridSize);
    }

    private void Update()
    {
        if (_isPlacing && _flyingBuilding != null)
        {
            MoveBuildingWithCursor();

            if (Input.GetMouseButtonDown(0))
            {
                TryPlaceBuilding();
            }
        }

        if (_isDeleting && Input.GetMouseButtonDown(0))
        {
            TryDeleteBuilding();
        }
    }

    private void MoveBuildingWithCursor()
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            int x = Mathf.RoundToInt(worldPosition.x);
            int y = Mathf.RoundToInt(worldPosition.z);

            bool available = IsPositionValid(x, y);

            _flyingBuilding.transform.position = new Vector3(x, 0, y);
            _flyingBuilding.SetTransparent(available);
        }
    }

    private bool IsPositionValid(int x, int y)
    {
        if (x < 0 || y < 0 || x > _gridSize.x - _flyingBuilding._size.x || y > _gridSize.y - _flyingBuilding._size.y)
            return false;

        return !IsPlaceTaken(x, y);
    }

    private bool IsPlaceTaken(int x, int y)
    {
        for (int i = 0; i < _flyingBuilding._size.x; i++)
        {
            for (int j = 0; j < _flyingBuilding._size.y; j++)
            {
                if (_grid[x + i, y + j] != null)
                    return true;
            }
        }
        return false;
    }

    private void TryPlaceBuilding()
    {
        Vector3 pos = _flyingBuilding.transform.position;
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.z);

        if (IsPositionValid(x, y))
        {
            PlaceBuilding(x, y);
        }
    }

    private void PlaceBuilding(int x, int y)
    {
        _flyingBuilding.SetNormal();

        for (int i = 0; i < _flyingBuilding._size.x; i++)
        {
            for (int j = 0; j < _flyingBuilding._size.y; j++)
            {
                _grid[x + i, y + j] = _flyingBuilding;
            }
        }

        _flyingBuilding = null;
        _isPlacing = false;
        SaveBuildingsManager.SaveBuildings(_grid, _buildingPrefabs, _gridSize);
    }

    private void TryDeleteBuilding()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
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

        for (int i = 0; i < building._size.x; i++)
        {
            for (int j = 0; j < building._size.y; j++)
            {
                _grid[x + i, y + j] = null;
            }
        }
        Destroy(building.gameObject);
        SaveBuildingsManager.SaveBuildings(_grid, _buildingPrefabs, _gridSize);
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (_flyingBuilding != null)
            Destroy(_flyingBuilding.gameObject);

        _flyingBuilding = Instantiate(buildingPrefab);
        _flyingBuilding.name = buildingPrefab.name;
        _isPlacing = true;
        _isDeleting = false; // Отключаем режим удаления
    }

    public void StartDeletingMode(bool isDeleting)
    {
        _isDeleting = isDeleting;
        _isPlacing = false; // Отключаем режим размещения
    }
}
