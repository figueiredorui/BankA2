using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankA.Data.Repositories;
using BankA.Models.Transactions;
using System.IO;
using Microsoft.AspNetCore.Http;
using BankA.Models.Tags;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankA.Api.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : BaseController
    {
        private readonly TagRepository tagRepository;

        public TagsController()
        {
            this.tagRepository = new TagRepository(this.User?.Identity?.Name);
        }


        // GET: api/tags
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var lst = tagRepository.Get();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/tags/Lookup
        [HttpGet("Lookup")]
        public IActionResult GetLookup()
        {
            try
            {
                var lst = tagRepository.GetLookup();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/Tags/5
        [HttpGet("{id}", Name = "GetTag")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = tagRepository.Get(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TagInfo tag)
        {
            try
            {
                tag = tagRepository.Update(tag);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // POST: api/Tags
        [HttpPost]
        public IActionResult Post([FromBody]TagInfo tag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                tag = tagRepository.Add(tag);
                return CreatedAtRoute("GetTag", new { id = tag.TagId }, tag);
                //return Ok(tag);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                tagRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
