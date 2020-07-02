using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferID { get; set; }
        public int TransferTypeID { get; set; }
        public int TransferStatusID { get; set; }
        public int FromAccountID { get; set; }
        public int ToAccountID { get; set; }
        public decimal TransferAmount { get; set; }

    }
}
