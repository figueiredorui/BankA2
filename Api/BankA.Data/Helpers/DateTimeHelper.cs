using System;
using System.Collections.Generic;
using System.Text;

namespace BankA.Data.Helpers
{
    class DateTimeHelper
    {
        public static DateTime StartPriod(int period)
        {
            var date = DateTime.Now.AddMonths(-period);
            return new DateTime(date.Year, date.Month, 1);
        }
    }
}
