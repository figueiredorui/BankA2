using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankA.Models.Accounts
{
    public class AccountSummary
    {
        public int? AccountID { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public DateTime? LastTransactionDate { get; set; }
    }
}
