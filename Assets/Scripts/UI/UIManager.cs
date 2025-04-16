using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Preview UI")]
    public GameObject _previewPanel;
    public Image _previewImage;
    public Text _costText;
    public Text _incomeText;

    public Button _placeButton;
    public Button _deleteButton;
    public Button[] _buildingButtons;

    private Building[] _buildingPrefabs;
    private BuildingGrid _buildingGrid;
    private Building _selectedBuildingPrefab; // Выбранный префаб здания

    private void Start()
    {
        _buildingPrefabs = PrefabsManager.Instance.GetPrefabs();
        _buildingGrid = FindObjectOfType<BuildingGrid>();

        for (int i = 0; i < _buildingButtons.Length; i++)
        {
            int index = i;
            _buildingButtons[i].onClick.AddListener(() => SelectBuilding(index));
            var trigger = _buildingButtons[i].gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

            AddTrigger(trigger, UnityEngine.EventSystems.EventTriggerType.PointerEnter, () => ShowPreview(index));
            AddTrigger(trigger, UnityEngine.EventSystems.EventTriggerType.PointerExit, HidePreview);
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

    private void ShowPreview(int index)
    {
        BuildingInfo info = _buildingPrefabs[index].Info;
        if (info == null) return;

        _previewImage.sprite = info.Icon;
        _costText.text = $"Стоимость: {info.Cost}";
        _incomeText.text = $"Доход: {info.IncomePerTick} / {info.Interval} сек";
        _previewPanel.SetActive(true);
    }

    private void HidePreview()
    {
        _previewPanel.SetActive(false);
    }

    private void AddTrigger(UnityEngine.EventSystems.EventTrigger trigger, UnityEngine.EventSystems.EventTriggerType eventType, System.Action action)
    {
        var entry = new UnityEngine.EventSystems.EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(_ => action());
        trigger.triggers.Add(entry);
    }

}
