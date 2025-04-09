public class WalletModel
{
    public int Balance { get; set; }

    public void Add(int amount)
    {
        Balance += amount;
    }

    public void Sub(int amount)
    {
        Balance -= amount;
    }

    public void Set(int amount)
    {
        Balance = amount;
    }
}
