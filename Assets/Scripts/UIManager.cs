using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button placeButton;
    public Button deleteButton;
    public Button[] buildingButtons;
    public Building[] buildingPrefabs;
    private BuildingGrid buildingGrid;
    //private bool isDeleting = false;

    private void Start()
    {
        buildingGrid = FindObjectOfType<BuildingGrid>();

        for (int i = 0; i < buildingButtons.Length; i++)
        {
            int index = i;
            buildingButtons[i].onClick.AddListener(() => SelectBuilding(index));
        }

        placeButton.onClick.AddListener(() => buildingGrid.StartDeletingMode(false));
        deleteButton.onClick.AddListener(() => buildingGrid.StartDeletingMode(true));
    }

    private void SelectBuilding(int index)
    {
        buildingGrid.StartPlacingBuilding(buildingPrefabs[index]);
    }
}
