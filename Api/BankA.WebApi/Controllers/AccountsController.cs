using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankA.Data.Repositories;
using BankA.Models.Accounts;
using AutoMapper;
using BankA.Models.Exceptions;
using BankA.Models.Transactions;
using Microsoft.AspNetCore.Http;
using System.IO;
using BankA.Models.Files;

namespace BankA.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : BaseController
    {
        private readonly AccountRepository accountRepository;
        private readonly FilesRepository fileRepository;
        private readonly TransactionRepository transactionRepository;

        public AccountsController()
        {
            this.accountRepository = new AccountRepository(this.User?.Identity?.Name);
            this.fileRepository = new FilesRepository(this.User?.Identity?.Name);
            this.transactionRepository = new TransactionRepository(this.User?.Identity?.Name);
        }

        // GET: api/Accounts
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var lst = accountRepository.GetList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/Accounts/5/Summary
        [HttpGet("{id}/Summary")]
        public IActionResult GetSummary(int id)
        {
            try
            {
                var result = transactionRepository.GetAccountSummary(id, 12);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/Accounts/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = accountRepository.Get(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/Accounts/5/CashFlow
        [HttpGet("{id}/CashFlow")]
        public IActionResult GetCashFlow(int id)
        {
            try
            {
                var result = transactionRepository.GetMonthlyCashFlow(id, 12);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/Accounts/5/Expenses
        //[HttpGet("{id}/Expenses")]
        //public IActionResult GetExpenses(int id)
        //{
        //    try
        //    {
        //        var result = transactionRepository.GetExpenses(id, DateTime.MinValue, DateTime.MaxValue);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException(ex);
        //    }
        //}

        // GET: api/Accounts/5/TagDetails
        [HttpGet("{id}/TagDetails")]
        public IActionResult TagDetails(int id)
        {
            try
            {
                var result = transactionRepository.GetTagDetails(id, 12);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}/Top10Expenses")]
        public IActionResult Top10Expenses(int id)
        {
            try
            {
                var result = transactionRepository.GetTop10Expenses(id, 12);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Account account)
        {
            try
            {
                account = accountRepository.Update(account);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // POST: api/Accounts
        [HttpPost]
        public IActionResult Post([FromBody]Account account)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                account = accountRepository.Add(account);
                return CreatedAtRoute("Get", new { id = account.AccountId }, account);
                //return Ok(account);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                accountRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/Accounts/5/transactions
        [HttpGet("{id}/transactions")]
        public IActionResult GetTransactions(int id, [FromQuery]TransactionSearch search)
        {
            try
            {
                var result = transactionRepository.GetTransactions(id, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // POST: api/Accounts/5/transactions/import
        [HttpPost("{id}/transactions/import")]
        public IActionResult ImportFile(int id, ImportCsvDefinition importCsvDefinition, IFormFile formFile)
        {
            try
            {
                if (formFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);

                        var fileImport = new FileImport()
                        {
                            FileName = formFile.FileName,
                            ContentType = formFile.ContentType,
                            FileContent = memoryStream.ToArray()
                        };

                        this.fileRepository.ImportFile(id, importCsvDefinition, fileImport);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Accounts/5/files
        [HttpGet("{accountId}/files")]
        public IActionResult GetFiles(int accountId)
        {
            try
            {
                var result = fileRepository.GetFiles(accountId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // DELETE: api/Accounts/5/Files/6
        [HttpDelete("{accountId}/files/{id}")]
        public IActionResult Delete(int accountId, int id)
        {
            try
            {
                fileRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/Accounts/5/files
        //[HttpGet("{accountId}/tags")]
        //public IActionResult GetTags(int accountId)
        //{
        //    try
        //    {
        //        var result = transactionRepository.GetTags(accountId);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException(ex);
        //    }
        //}
    }
}
