using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button _placeButton;
    public Button _deleteButton;
    public Button[] _buildingButtons;

    public GameObject _buildingPreviewPanel;
    public Image _buildingImage;
    public Text _incomeText;
    public Text _costText;

    private Building[] _buildingPrefabs;
    private BuildingGrid _buildingGrid;
    private Building _selectedBuildingPrefab;

    private void Start()
    {
        _buildingPrefabs = PrefabsManager.Instance.GetPrefabs();
        _buildingGrid = FindObjectOfType<BuildingGrid>();

        for (int i = 0; i < _buildingButtons.Length; i++)
        {
            int index = i;
            _buildingButtons[i].onClick.AddListener(() => SelectBuilding(index));
        }

        _placeButton.onClick.AddListener(StartPlacingSelectedBuilding);
        _deleteButton.onClick.AddListener(() =>
        {
            _buildingGrid.StartDeletingMode(true);
            _buildingPreviewPanel.SetActive(false);
        });

        _buildingPreviewPanel.SetActive(false); // изначально скрыта
    }

    private void SelectBuilding(int index)
    {
        _selectedBuildingPrefab = _buildingPrefabs[index];
        ShowPreview(index);
    }

    private void StartPlacingSelectedBuilding()
    {
        if (_selectedBuildingPrefab != null)
        {
            _buildingGrid.StartPlacingBuilding(_selectedBuildingPrefab);
            _buildingPreviewPanel.SetActive(false); // скрываем панель
        }
    }

    private void ShowPreview(int index)
    {
        BuildingInfo info = _buildingPrefabs[index].Info;
        if (info != null)
        {
            _buildingImage.sprite = info.Icon;
            _costText.text = $"Стоимость: {info.Cost}";
            _incomeText.text = $"Доход: {info.IncomePerTick}/сек";
            _buildingPreviewPanel.SetActive(true);
        }
    }
}