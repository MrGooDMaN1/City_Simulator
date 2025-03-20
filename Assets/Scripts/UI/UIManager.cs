using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button _placeButton;
    public Button _deleteButton;
    public Button[] _buildingButtons;
    public Building[] _buildingPrefabs;

    private BuildingGrid _buildingGrid;

    private void Start()
    {
        _buildingGrid = FindObjectOfType<BuildingGrid>();

        for (int i = 0; i < _buildingButtons.Length; i++)
        {
            int index = i;
            _buildingButtons[i].onClick.AddListener(() => SelectBuilding(index));
        }

        
        _placeButton.onClick.AddListener(() => _buildingGrid.StartDeletingMode(false));
        _deleteButton.onClick.AddListener(() => _buildingGrid.StartDeletingMode(true));
    }

    private void SelectBuilding(int index)
    {
        _buildingGrid.StartPlacingBuilding(_buildingPrefabs[index]);
    }
}