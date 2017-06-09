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
    public class FilesController : BaseController
    {
        private readonly FilesRepository filesRepository;

        public FilesController()
        {
            this.filesRepository = new FilesRepository(this.User?.Identity?.Name);
        }

        // POST: api/files/parse
        [HttpPost("parse")]
        public IActionResult ParseFile(IFormFile formFile)
        {
            try
            {
                if (formFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);
                        var array = memoryStream.ToArray();
                        var result = this.filesRepository.ParseFile(array);

                        return Ok(result);
                    }
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
