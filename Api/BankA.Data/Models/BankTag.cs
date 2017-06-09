using System;
using System.Collections.Generic;

namespace BankA.Data.Models
{
    class BankTag : EntityBase
    {
        public int TagId { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
    }
}
