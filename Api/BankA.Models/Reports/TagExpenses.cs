using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BankA.Models.Reports
{
    public partial class TagExpenses
    {
        public string Tag { get; set; }
        public decimal Amount { get; set; }
        public List<TagExpensesDetails> Details { get; set; }
    }

    public partial class TagExpensesDetails
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public string MonthYear { get { return $"{DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Month)}/{Year}"; } }
    }
}
