namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        decimal GetAccountBalance(int userID);
    }
}