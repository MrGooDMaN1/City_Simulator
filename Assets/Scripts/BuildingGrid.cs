using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);

    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPositin = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPositin.x);
                int y = Mathf.RoundToInt(worldPositin.z);

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

    private bool IsPlaceTaken(int placeX, int PlaceY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, PlaceY + y] != null)
                    return true;
            }
        }
        return false;
    }

    private void PlaceFlayingBuilding(int placeX, int PlaceY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++) 
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, PlaceY + y] = flyingBuilding;
            }
        }

        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if(flyingBuilding != null)
            Destroy(flyingBuilding.gameObject);

        flyingBuilding = Instantiate(buildingPrefab);
    }
}
