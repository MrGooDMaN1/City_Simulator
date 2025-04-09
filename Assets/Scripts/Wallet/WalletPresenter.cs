public class WalletPresenter
{
    private WalletModel _model;
    private WalletView _view;

    public WalletPresenter(WalletModel model, WalletView view)
    {
        _model = model;
        _view = view;

        _view.UpdateBalance(_model.Balance);
    }

    public void Sub(int value)
    {
        _model.Sub(value);
        _view.UpdateBalance(_model.Balance);
    }

    public void Add(int value)
    {
        _model.Add(value);
        _view.UpdateBalance(_model.Balance);
    }
}
