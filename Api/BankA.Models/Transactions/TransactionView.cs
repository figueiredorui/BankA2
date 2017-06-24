using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BankA.Models.Reports
{

    public partial class TransactionView
    {
        public int AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal TransferOutAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal TransferInAmount { get; set; }
    }
}
