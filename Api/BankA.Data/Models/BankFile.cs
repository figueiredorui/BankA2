using System;
using System.Collections.Generic;

namespace BankA.Data.Models
{
    class BankFile : EntityBase
    {
        public BankFile()
        {
            Transactions = new HashSet<BankTransaction>();
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public int AccountId { get; set; }

        public virtual ICollection<BankTransaction> Transactions { get; set; }
    }
}
