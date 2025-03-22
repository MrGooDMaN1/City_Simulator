using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private GameInput _gameInput;
    private BuildingGrid _buildingGrid;

    private void Awake()
    {
        _gameInput = new GameInput();
        _buildingGrid = FindObjectOfType<BuildingGrid>();

        _gameInput.Gameplay.Clik.performed += ctx => PlaceBuilding();
        _gameInput.Gameplay.Clik.performed += ctx => DeleteBuilding();
    }

    private void PlaceBuilding()
    {
        _buildingGrid.TryPlaceBuilding();
    }

    private void DeleteBuilding()
    {
        _buildingGrid.TryDeleteBuilding();
    }

    private void OnEnable() => _gameInput.Enable();
    private void OnDisable() => _gameInput.Disable();
}
