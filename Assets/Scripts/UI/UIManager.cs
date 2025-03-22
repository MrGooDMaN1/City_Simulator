using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button _placeButton;
    public Button _deleteButton;
    public Button[] _buildingButtons;
    public Building[] _buildingPrefabs;

    private BuildingGrid _buildingGrid;
    private Building _selectedBuildingPrefab; // Выбранный префаб здания

    private void Start()
    {
        _buildingGrid = FindObjectOfType<BuildingGrid>();

        for (int i = 0; i < _buildingButtons.Length; i++)
        {
            int index = i;
            _buildingButtons[i].onClick.AddListener(() => SelectBuilding(index));
        }

        _placeButton.onClick.AddListener(StartPlacingSelectedBuilding);
        _deleteButton.onClick.AddListener(() => _buildingGrid.StartDeletingMode(true));
    }

    private void SelectBuilding(int index)
    {
        _selectedBuildingPrefab = _buildingPrefabs[index]; // Запоминаем выбранное здание
    }

    private void StartPlacingSelectedBuilding()
    {
        if (_selectedBuildingPrefab != null)
        {
            _buildingGrid.StartPlacingBuilding(_selectedBuildingPrefab);
        }
    }
}
