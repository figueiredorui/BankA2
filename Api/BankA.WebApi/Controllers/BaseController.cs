using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankA.Data.Repositories;
using BankA.Models.Accounts;
using AutoMapper;
using BankA.Models.Exceptions;

namespace BankA.Api.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult HandleException(Exception ex)
        {
            if (ex is EntityNotFoundException)
            {
                return NotFound(ex.Message);
            }

            return BadRequest(ex.Message);
        }
    }
}
