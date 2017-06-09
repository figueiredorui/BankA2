using System;
using System.Collections.Generic;

namespace BankA.Models.Files
{
    public class FileImport
    {
        public FileImport()
        {
        }

        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileContent { get; set; }
    }
}
