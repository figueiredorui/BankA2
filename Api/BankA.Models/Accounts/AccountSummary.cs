using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankA.Models.Accounts
{
    public class AccountSummary
    {
        public int? AccountId { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal TransferOutAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal TransferInAmount { get; set; }
        public decimal Balance { get; set; }

        public DateTime FirstTransactionDate { get; set; }
        public DateTime LastTransactionDate { get; set; }
    }
}
