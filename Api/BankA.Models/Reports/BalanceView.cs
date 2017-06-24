using System;
using System.Collections.Generic;
using System.Text;

namespace BankA.Models.Reports
{
    public class BalanceView
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Date { get; set; }
        public decimal Balance { get; set; }
    }
}
