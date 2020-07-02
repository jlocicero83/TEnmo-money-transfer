﻿using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        decimal GetAccountBalance(string username);
        Transfer CreateTransfer(Transfer newTransfer);
        Transfer GetTransferByID(int id);
        void AdjustBalances(Transfer transfer);
    }
}