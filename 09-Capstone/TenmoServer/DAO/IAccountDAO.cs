using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        decimal GetAccountBalance(string username);
        Transfer CreateTransfer(Transfer newTransfer);
        Transfer GetTransferByID(int id);
        bool AdjustBalances(Transfer transfer);
        List<Transfer> GetAllTransfersByUser(int userID);
    }
}