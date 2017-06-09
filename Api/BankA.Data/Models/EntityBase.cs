using System;
using System.Collections.Generic;
using System.Text;

namespace BankA.Data.Models
{
    abstract class EntityBase : IEntityBase
    {
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
        public string ChangedBy { get; set; }
    }
}
