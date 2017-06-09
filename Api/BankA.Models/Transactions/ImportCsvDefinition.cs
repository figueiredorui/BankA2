using System;
using System.Collections.Generic;
using System.Text;

namespace BankA.Models.Transactions
{
    public class ImportCsvDefinition    {
        public bool HasHeaders { get; set; }
        public int TransactionDate_Index { get; set; }
        public int TransactionType_Index { get; set; }
        public int Description_Index { get; set; }
        public int DebitAmount_Index { get; set; }
        public int CreditAmount_Index { get; set; }
        public int Amount_Index { get; set; }
    }


}
