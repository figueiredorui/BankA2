using System;
using System.Collections.Generic;

namespace BankA.Models.Files
{
    public class FileImported
    {
        public FileImported()
        {
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int AccountId { get; set; }
        public DateTime ImportedOn { get; set; }

    }
}
