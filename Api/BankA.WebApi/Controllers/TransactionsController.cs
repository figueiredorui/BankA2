using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankA.Data.Repositories;
using BankA.Models.Transactions;
using System.IO;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankA.Api.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController : BaseController
    {
        private readonly TransactionRepository transactionsRepository;
        private readonly FilesRepository filesRepository;

        public TransactionsController()
        {
            this.transactionsRepository = new TransactionRepository(this.User?.Identity?.Name);
            this.filesRepository = new FilesRepository(this.User?.Identity?.Name);
        }


        // PUT: api/transactions/5/tag
        [HttpPut("{id}/tag")]
        public IActionResult Put(int id, [FromBody]string tag)
        {
            try
            {
                transactionsRepository.UpdateTag(id, tag);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
