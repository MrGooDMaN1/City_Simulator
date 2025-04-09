using UnityEngine;

public class WalletManager : MonoBehaviour
{
    public static WalletManager Instance { get; private set; }

    private WalletModel _model;
    private WalletView _view;
    private WalletPresenter _presenter;

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
    }

    public void AddMoney(int value)
    {
        _presenter.Add(value);
    }

    public bool TrySub(int value)
    {
        if (_model.Balance >= value)
        {
            _presenter.Sub(_model.Balance);
            return true;
        }
        else
            return false;
    }

}
