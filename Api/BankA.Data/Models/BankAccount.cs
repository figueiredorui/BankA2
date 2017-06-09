using System;
using System.Collections.Generic;

namespace BankA.Data.Models
{
    class BankAccount : EntityBase
    {
        public BankAccount()
        {
            Transactions = new HashSet<BankTransaction>();
        }

        public int AccountId { get; set; }
        public string Description { get; set; }
        public string ImportCsvDefinition { get; set; }
        public bool Closed { get; set; }
        
        public virtual ICollection<BankTransaction> Transactions { get; set; }
    }
}
