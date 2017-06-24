using BankA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankA.Models.Accounts;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using BankA.Models.Reports;
using BankA.Models.Transactions;
using System.Linq.Expressions;
using BankA.Models.Tags;
using BankA.Data.Helpers;

namespace BankA.Data.Repositories
{
    public class AccountRepository : Repository
    {
        public AccountRepository(string currentUser):base(currentUser)
        {
        }

        public List<Account> GetList()
        {
            var result = base.Table<BankAccount>()
                    .Select(s => new
                    {
                        AccountId = s.AccountId,
                        Description = s.Description,
                        Closed = s.Closed,
                        Balance = (decimal?)s.Transactions.Sum(sum => sum.CreditAmount - sum.DebitAmount) ?? 0
                    }).ProjectTo<Account>().ToList();

            result.Add(new Account()
            {
                AccountId = 0,
                Description = "All Accounts",
                Balance = result.Sum(q => q.Balance),
            });

            result = result.OrderBy(s => s.AccountId).ToList();

            return result;
        }

        //private decimal GetBalance(int accountId)
        //{
        //    var transactionsLst = base.Table<BankTransaction>()
        //        .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId));

        //    var balance = (decimal?)transactionsLst.Sum(sum => sum.CreditAmount - sum.DebitAmount) ?? 0;

        //    return balance;
        //}

        public Account Get(int id)
        {
            if (id == 0)
            {
                return new Account() { AccountId = 0, Description = "All Accounts" };
            }
            else
            {
                var result = base.Find<BankAccount>(id);
                return Mapper.Map<Account>(result);
            }
        }

        public Account Add(Account account)
        {
            var bankAccount = Mapper.Map<BankAccount>(account);

            var result = base.Add(bankAccount);

            return Mapper.Map<Account>(result);
        }

        public Account Update(Account account)
        {
            var bankAccount = base.Find<BankAccount>(account.AccountId);
            Mapper.Map(account, bankAccount);

            var result = base.Update(bankAccount);

            return Mapper.Map<Account>(result);
        }

        public void Delete(int id)
        {
            base.Delete<BankAccount>(id);
        }

        
    }
}
