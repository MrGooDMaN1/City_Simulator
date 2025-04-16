using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickLoadScene : MonoBehaviour
{
    [SerializeField] private Button _loadingButton;
    [SerializeField] private int _sceneIndex;

    private void Start()
    {
        _loadingButton.onClick.AddListener(() => SceneManager.LoadScene(_sceneIndex));
    }
}
