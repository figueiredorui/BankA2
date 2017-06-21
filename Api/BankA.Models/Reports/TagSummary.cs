using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BankA.Models.Reports
{
    public partial class TagSummary
    {
        public string Type { get; set; }
        public string Tag { get; set; }
        public decimal Amount { get; set; }
        public List<TagSummaryDetails> Details { get; set; }
    }

    public partial class TagSummaryDetails
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public string MonthYear { get { return $"{DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Month)}/{Year}"; } }
    }
}
