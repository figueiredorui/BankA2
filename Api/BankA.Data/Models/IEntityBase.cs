using System;

namespace BankA.Data.Models
{
    public interface IEntityBase
    {
        string ChangedBy { get; set; }
        DateTime? ChangedOn { get; set; }
        string CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
    }
}