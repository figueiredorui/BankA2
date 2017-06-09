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
        public string MonthYear { get { return String.Format("{0}/{1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month).Substring(0, 3), Year); } }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }
}
