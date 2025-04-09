using UnityEngine.UI;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private Text _balanceText;

    public void UpdateBalance(int value)
    {
        _balanceText.text = $"{value}";
    }
}
