using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public Vector2Int _gridSize = new Vector2Int(10, 10);
    public Building[] _buildingPrefabs;

    public Building[,] _grid;

    private Building _flyingBuilding;
    private Camera _mainCamera;
    private bool _isDeleting = false;

    private void Awake()
    {
        _grid = new Building[_gridSize.x, _gridSize.y];
        _mainCamera = Camera.main;
        SaveBuildingsManager.LoadBuildings(_grid, _buildingPrefabs, _gridSize);
    }

    private void Update()
    {
        if (_isDeleting && Input.GetMouseButtonDown(0))
        {
            TryDeleteBuilding();
        }

        if (_flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > _gridSize.x - _flyingBuilding._size.x)
                    available = false;
                if (y < 0 || y > _gridSize.y - _flyingBuilding._size.y)
                    available = false;

                if (available && IsPlaceTaken(x, y))
                    available = false;

                _flyingBuilding.transform.position = new Vector3(x, 0, y);
                _flyingBuilding.SetTransparent(available);

                if (available && Input.GetMouseButtonDown(0))
                    PlaceFlyingBuilding(x, y);
            }
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingBuilding._size.x; x++)
        {
            for (int y = 0; y < _flyingBuilding._size.y; y++)
            {
                if (_grid[placeX + x, placeY + y] != null)
                    return true;
            }
        }
        return false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        _flyingBuilding.SetNormal();

        for (int x = 0; x < _flyingBuilding._size.x; x++)
        {
            for (int y = 0; y < _flyingBuilding._size.y; y++)
            {
                _grid[placeX + x, placeY + y] = _flyingBuilding;
            }
        }

        _flyingBuilding = null;
        SaveBuildingsManager.SaveBuildings(_grid, _buildingPrefabs, _gridSize);
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (_flyingBuilding != null)
            Destroy(_flyingBuilding.gameObject);

        _flyingBuilding = Instantiate(buildingPrefab);
        _flyingBuilding.name = buildingPrefab.name;
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

    public void StartDeletingMode(bool isDeleting)
    {
        _isDeleting = isDeleting;
    }
}
