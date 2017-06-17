using System;
using System.Collections.Generic;
using System.Text;

namespace BankA.Models.Reports
{
    public partial class ExpensesReport
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Tag { get; set; }
        public decimal Amount { get; set; }
    }
}
