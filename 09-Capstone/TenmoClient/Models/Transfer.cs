using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public Transfer(int fromID, int toID, decimal amount)
        {
            FromAccountID = fromID;
            ToAccountID = toID;
            TransferAmount = amount;
        }
        public int TransferID { get; set; }
        public int TransferTypeID { get; set; } = 2;
        public int TransferStatusID { get; set; } = 2;
        public int FromAccountID { get; set; }
        public int ToAccountID { get; set; }
        public decimal TransferAmount { get; set; }

    }
}
