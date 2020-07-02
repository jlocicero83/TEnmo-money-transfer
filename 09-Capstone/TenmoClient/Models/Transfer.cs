using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public enum TransferType
    {
        Request = 1,
        Send = 2
    }

    public enum TransferStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }

    public class Transfer
    {
        public Transfer(int fromID, int toID, decimal amount)
        {
            FromAccountID = fromID;
            ToAccountID = toID;
            TransferAmount = amount;
        }
        public Transfer()
        {

        }
        public int TransferID { get; set; }
        public TransferType TransferType { get; set; } = TransferType.Send;
        public TransferStatus TransferStatus { get; set; } = TransferStatus.Approved;
        public int FromAccountID { get; set; }
        public int ToAccountID { get; set; }
        public decimal TransferAmount { get; set; }

    }
}
