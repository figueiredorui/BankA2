using BankA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankA.Models.Transactions;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace BankA.Data.Repositories
{
    public class TransactionsRepository : Repository
    {

        public TransactionsRepository(string currentUser):base(currentUser)
        {
        }

        public void UpdateTag(int transactionId, string tag)
        {
            var transaction = base.Find<BankTransaction>(transactionId);
            transaction.Tag = tag;
            base.Update(transaction);
        }


    }
}
