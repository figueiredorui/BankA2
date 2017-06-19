using System;
using System.Collections.Generic;

namespace BankA.Models.Transactions
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Tag { get; set; }
        public bool IsTransfer { get; set; }

        public decimal Balance { get { return CreditAmount - DebitAmount; } }
    }
}
