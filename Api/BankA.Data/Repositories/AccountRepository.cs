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

        public AccountSummary GetAccountSummary(int accountId)
        {
            var result = new AccountSummary();

            if (accountId > 0)
            {
                result = base.Table<BankAccount>()
                            .Where(q => q.AccountId == accountId)
                            .Select(s => new
                            {
                                AccountId = s.AccountId,
                                Description = s.Description,
                            }).ProjectTo<AccountSummary>().FirstOrDefault();
            }
            else
            {
                result.AccountId = 0;
                result.Description = "All Accounts";
            }

            var transactions = base.Table<BankTransaction>().Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId));

            result.CreditAmount = (decimal?)transactions.Sum(sum => sum.CreditAmount) ?? 0;
            result.DebitAmount = (decimal?)transactions.Sum(sum => sum.DebitAmount) ?? 0;
            result.FirstTransactionDate = (DateTime?)transactions.Min(m => m.TransactionDate) ?? DateTime.MinValue;
            result.LastTransactionDate = (DateTime?)transactions.Max(m => m.TransactionDate) ?? DateTime.MinValue;

            return result;
        }


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
