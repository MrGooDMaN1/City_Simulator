using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public static Bootstrap Instance { get; private set; }
    public BuildingGrid buildingGrid { get; private set; }
    public UIManager UIManager { get; private set; }
    public InputManager inputManager { get; private set; }
    public PrefabsManager prefabsManager { get; private set; }

    private Building[] buildingPrefabs;

    private void Awake()
    {
        buildingGrid = GetComponent<BuildingGrid>();
        UIManager = GetComponent<UIManager>();
        inputManager = GetComponent<InputManager>();
        prefabsManager = GetComponent<PrefabsManager>();


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
        SaveBuildingsManager.LoadBuildings(buildingGrid._grid, buildingPrefabs, buildingGrid._gridSize);
    }
}

