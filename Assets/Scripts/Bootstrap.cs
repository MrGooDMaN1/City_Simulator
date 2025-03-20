using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public static Bootstrap Instance { get; private set; }

    [SerializeField] private BuildingGrid _buildingGrid;
    [SerializeField] private Building[] _buildingPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        SaveBuildingsManager.LoadBuildings(_buildingGrid._grid, _buildingPrefabs, _buildingGrid._gridSize);
    }
}

