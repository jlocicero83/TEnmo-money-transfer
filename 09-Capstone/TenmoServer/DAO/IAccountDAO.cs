namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        decimal GetAccountBalance(string username);
        bool TransferMoney(int recipientID, int senderID, decimal transferAmount);
    }
}