using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BankA.Models.Reports
{
    public partial class MonthlyCashFlow
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthYear { get { return $"{DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Month)}/{Year}"; } }
        public decimal DebitAmount { get; set; }
        public decimal TransferOutAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal TransferInAmount { get; set; }
        public decimal Balance { get; set; }

    }
}
