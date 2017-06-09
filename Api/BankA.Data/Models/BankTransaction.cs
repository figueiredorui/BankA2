using System;
using System.Collections.Generic;

namespace BankA.Data.Models
{
    class BankTransaction : EntityBase
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Tag { get; set; }
        public int? FileId { get; set; }

        public virtual BankAccount Account { get; set; }
        public virtual BankFile File { get; set; }

    }
}
