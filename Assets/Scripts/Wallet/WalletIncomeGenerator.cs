using UnityEngine;

public class WalletIncomeGenerator : MonoBehaviour
{
    [SerializeField] private float _interval = 3f;
    [SerializeField] private int _incomePerTick = 10;
    private bool _isActive = false;

    private void Start()
    {
        InvokeRepeating(nameof(GenerateIncome), _interval, _interval);
        Debug.Log("Делаем денюшку");
    }

    public void EnableIncome()
    {
        if (!_isActive)
        {
            _isActive = true;
            InvokeRepeating(nameof(GenerateIncome), _interval, _interval);
            Debug.Log("Делаем денюшку2");
        }
    }

    private void GenerateIncome()
    {
        if (WalletManager.Instance != null)
        {
            WalletManager.Instance.AddMoney(_incomePerTick);
        }
    }


}
