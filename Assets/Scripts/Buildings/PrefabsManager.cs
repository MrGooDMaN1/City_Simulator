using UnityEngine;

public class PrefabsManager : MonoBehaviour
{
    public static PrefabsManager Instance { get; private set; }

    [SerializeField] private Building[] _buildingPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Building[] GetPrefabs()
    {
        return _buildingPrefabs;
    }
}
