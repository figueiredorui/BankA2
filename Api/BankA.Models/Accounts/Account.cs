using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankA.Models.Accounts
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Description { get; set; }
        public string ImportCsvDefinition { get; set; }
        public bool Closed { get; set; }
    }
}
