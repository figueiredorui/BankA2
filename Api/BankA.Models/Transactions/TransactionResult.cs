using System;
using System.Collections.Generic;
using System.Text;

namespace BankA.Models.Transactions
{
    public class TransactionResult
    {
        public int ItemsPerPage { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public List<Transaction> Data { get; set; }
    }
}
