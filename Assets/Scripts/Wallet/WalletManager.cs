using UnityEngine;

public class WalletManager : MonoBehaviour
{
    public static WalletManager Instance { get; private set; }

    private WalletModel _model;
    private WalletView _view;
    private WalletPresenter _presenter;

    private int _savedBalance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _model = new WalletModel();
        _view = FindObjectOfType<WalletView>();
        _presenter = new WalletPresenter(_model, _view);

        _savedBalance = SaveManager.LoadWalletBalance();
        SetBalance(_savedBalance);
    }

    public void AddMoney(int value)
    {
        _presenter.Add(value);
        SaveManager.SaveWalletBalance(_model.Balance);
    }

    public bool TrySub(int value)
    {
        if (_model.Balance >= value)
        {
            _presenter.Sub(value);
            SaveManager.SaveWalletBalance(_model.Balance);
            return true;
        }
        else
            return false;
    }

    public int GetBalance()
    {
        return _model.Balance;
    }

    public void SetBalance(int amount)
    {
        _model.Set(amount);
        _view.UpdateBalance(_model.Balance);
    }

}
