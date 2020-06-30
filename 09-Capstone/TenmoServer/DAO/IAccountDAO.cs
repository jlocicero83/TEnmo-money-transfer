namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        decimal GetAccountBalance(string username);
    }
}