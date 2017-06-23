using BankA.Models.Transactions;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using BankA.Data.Models;
using BankA.Models.Files;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace BankA.Data.Repositories
{
    public class FilesRepository : Repository
    {
        public FilesRepository(string currentUser) : base(currentUser)
        {
        }

        public List<FileImported> GetFiles(int accountId)
        {
            var result = base.Table<BankFile>().Where(q => q.AccountId == accountId).ProjectTo<FileImported>().ToList();
            return result;
        }

        public void ImportFile(int accountId, ImportCsvDefinition importCsvDefinition, FileImport fileImport)
        {
            VaidateImportColumnIndex(importCsvDefinition);
            var transactionList = ParseFile(importCsvDefinition, fileImport.FileContent);
            SaveFile(accountId, importCsvDefinition, fileImport, transactionList);
            UpdateImportCsvDefinition(accountId, importCsvDefinition);
        }

        public object ParseFile(byte[] fileContent)
        {
            try
            {
                var lst = new List<JObject>();

                using (var csv = InitReader(fileContent))
                {
                    while (csv.Read())
                    {
                        var jsonObject = new JObject();
                        for (int i = 0; i < csv.CurrentRecord.Length; i++)
                        {
                            jsonObject.Add($"Col{i}", csv.CurrentRecord[i]);

                        }
                        lst.Add(jsonObject);
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Delete(int id)
        {
            base.Delete<BankFile>(id);
        }

        private void SaveFile(int accountId, ImportCsvDefinition importCsvDefinition, FileImport fileImport, List<BankTransaction> transactionList)
        {
            var file = new BankFile();
            file.AccountId = accountId;
            file.FileContent = fileImport.FileContent;
            file.ContentType = fileImport.ContentType;
            file.FileName = fileImport.FileName;

            AddTransactions(file, transactionList);

            ApplyBestRule(file.Transactions);

            base.Add(file);
        }

        private void AddTransactions(BankFile file, List<BankTransaction> transactionList)
        {
            var lastTransactionDate = base.Table<BankTransaction>().Where(q => q.AccountId == file.AccountId).Max(m => m.TransactionDate);

            foreach (var item in transactionList.Where(q => q.TransactionDate > lastTransactionDate))
            {
                item.AccountId = file.AccountId;
                file.Transactions.Add(item);
            }
        }

        private List<BankTransaction> ParseFile(ImportCsvDefinition importCsvDefinition, byte[] fileContent)
        {
            try
            {
                var lst = new List<BankTransaction>();

                using (var csv = InitReader(fileContent))
                {
                    while (csv.Read())
                    {
                        if (importCsvDefinition.HasHeaders && csv.Row == 1)
                            continue;

                        var transaction = new BankTransaction();

                        transaction.TransactionDate = csv.GetField<DateTime>(importCsvDefinition.TransactionDate_Index);
                        transaction.TransactionType = ParseTransactionType(csv, importCsvDefinition.TransactionType_Index);
                        transaction.Description = csv.GetField<string>(importCsvDefinition.Description_Index);
                        transaction.Tag = csv.GetField<string>(importCsvDefinition.Tag_Index);

                        if (importCsvDefinition.Amount_Index > -1) {
                            var amount = ParseAmount(csv, importCsvDefinition.Amount_Index);
                            if (amount > 0)
                                transaction.CreditAmount = amount;
                            else
                                transaction.DebitAmount = Math.Abs(amount);
                        }
                        else
                        {
                            transaction.CreditAmount = ParseAmount(csv, importCsvDefinition.CreditAmount_Index);
                            transaction.DebitAmount = ParseAmount(csv, importCsvDefinition.DebitAmount_Index);
                        }
                       

                        lst.Add(transaction);
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string ParseTransactionType(CsvReader csv, int index)
        {
            var str = "";

            if (index > -1)
                str = csv.GetField(index);

            return str;
        }

        private CsvReader InitReader(byte[] fileContent)
        {
            var reader = new StreamReader(new MemoryStream(fileContent));
            var csv = new CsvReader(reader);

            csv.Configuration.IgnoreReadingExceptions = false;
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.IgnoreQuotes = false;
            csv.Configuration.ReadingExceptionCallback = (ex, row) =>
            {
                throw ex;
            };

            return csv;
        }

        private decimal ParseAmount(CsvReader csv, int index)
        {
            var str = csv.GetField(index);
            str = str.Trim('"');
            str = str.Trim('\'');

            decimal amount = 0;
            var valid = decimal.TryParse(str, out amount);
            return amount;
        }

        private void ApplyBestRule(ICollection<BankTransaction> transactionLst)
        {
            var transactionRules = base.Table<BankTag>();

            foreach (var transaction in transactionLst)
            {
                var rule = transactionRules.Where(q => transaction.Description.ToUpper().Contains(q.Description.ToUpper())).FirstOrDefault();
                if (rule != null)
                {
                    transaction.Tag = rule.Tag;
                }
            }
        }

        private void VaidateImportColumnIndex(ImportCsvDefinition importCsvDefinition)
        {
            if (importCsvDefinition.TransactionDate_Index < 0)
                throw new Exception("TransactionDate mapping is required");


            if (importCsvDefinition.Description_Index < 0)
                throw new Exception("Description mapping is required");


            if (importCsvDefinition.Amount_Index < 0 && importCsvDefinition.CreditAmount_Index < 0 && importCsvDefinition.DebitAmount_Index < 0)
                throw new Exception("Amount or CreditAmount/DebitAmount mapping is required");

            //TODO
            //more validations
        }

        private void UpdateImportCsvDefinition(int accountId, ImportCsvDefinition importCsvDefinition)
        {
            var account = base.Find<BankAccount>(accountId);
            account.ImportCsvDefinition = JsonConvert.SerializeObject(importCsvDefinition);
            base.Update(account);
        }

        public static class ImportOfx
        {
            public static XElement toXElement(string pathToOfxFile)
            {
                if (!System.IO.File.Exists(pathToOfxFile))
                {
                    throw new FileNotFoundException();
                }

                //use LINQ TO GET ONLY THE LINES THAT WE WANT
                var tags = from line in File.ReadAllLines(pathToOfxFile)
                           where line.Contains("<STMTTRN>") ||
                           line.Contains("<TRNTYPE>") ||
                          line.Contains("<DTPOSTED>") ||
                          line.Contains("<TRNAMT>") ||
                           line.Contains("<FITID>") ||
                          line.Contains("<CHECKNUM>") ||
                          line.Contains("<MEMO>")
                           select line;


                XElement el = new XElement("root");
                XElement son = null;
                //StreamWriter sr= new StreamWriter(@"c:\rodrigo\teste.txt");
                foreach (var l in tags)
                {
                    if (l.IndexOf("<STMTTRN>") != -1)
                    {
                        son = new XElement("STMTTRN");
                        el.Add(son);
                        continue;
                    }

                    var tagName = getTagName(l);
                    var elSon = new XElement(tagName);
                    elSon.Value = getTagValue(l);
                    son.Add(elSon);
                }
                //using (StreamWriter sr = new StreamWriter(@"c:\rodrigo\teste.xml"))
                //{
                //    sr.WriteLine(el.ToString());
                //    sr.Flush();
                //    sr.Close();
                //}
                return el;

            }
            /// <summary>
            /// Get the Tag name to create an Xelement
            /// </summary>
            /// <param name="line">One line from the file</param>
            /// <returns></returns>
            private static string getTagName(string line)
            {
                int pos_init = line.IndexOf("<") + 1;
                int pos_end = line.IndexOf(">");
                pos_end = pos_end - pos_init;
                return line.Substring(pos_init, pos_end);
            }
            /// <summary>
            /// Get the value of the tag to put on the Xelement
            /// </summary>
            /// <param name="line">The line</param>
            /// <returns></returns>
            private static string getTagValue(string line)
            {
                int pos_init = line.IndexOf(">") + 1;
                string retValue = line.Substring(pos_init).Trim();
                if (retValue.IndexOf("[") != -1)
                {
                    //date--lets get only the 8 date digits
                    retValue = retValue.Substring(0, 8);
                }
                return retValue;
            }
        }
    }
}


