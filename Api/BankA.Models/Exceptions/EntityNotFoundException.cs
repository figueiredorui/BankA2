using System;
using System.Collections.Generic;
using System.Text;

namespace BankA.Models.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
        : base(message)
    {
        }

        public EntityNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
        }
    }
}
