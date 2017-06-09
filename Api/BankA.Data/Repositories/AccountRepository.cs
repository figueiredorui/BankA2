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
            return base.Table<BankAccount>()
                    .ProjectTo<Account>().ToList();
        }

        public List<AccountSummary> GetAccountSummary()
        {
            var result = base.Table<BankAccount>()
                            .Select(s => new
                            {
                                AccountID = s.AccountId,
                                Description = s.Description,
                                Balance = (decimal?)s.Transactions.Sum(sum => sum.CreditAmount - sum.DebitAmount) ?? 0,
                                LastTransactionDate = (DateTime?)s.Transactions.Max(m => m.TransactionDate) ?? DateTime.MinValue
                            }).ToList();

            //TODO
            //replace by ProjectTo (v1.1.2)
            //https://github.com/aspnet/EntityFramework/issues/7714
            return result.Select(s=> Mapper.Map<AccountSummary>(s)).ToList();
        }

        public TransactionResult GetTransactions(int accountId, TransactionSearch search)
        {
            if (search.Query == null)
                search.Query = string.Empty;

            int pageSize = 20;
            var transactions = base.Table<BankTransaction>()
                        .Where(q => q.AccountId == accountId 
                                && q.TransactionDate <= DateTime.Now
                                && (q.Description.Contains(search.Query) || q.Tag.Contains(search.Query))) 
                        .OrderByDescending(o=>o.TransactionDate)
                        .ProjectTo<Transaction>().ToList();

            var result = new TransactionResult()
            {
                ItemsPerPage = pageSize,
                Page =search.Page,
                Count = transactions.Count,
                Data = transactions.Skip((search.Page-1) * pageSize).Take(pageSize).ToList()
            };

            return result;
        }

        public List<MonthlyCashFlow> GetMonthlyCashFlow(int accountID)
        {
            var transactionsLst = base.Table<BankTransaction>()
                                        .Where(q => q.AccountId == accountID
                                            && q.TransactionDate <= DateTime.Now);

            var result = (from item in transactionsLst
                          group item by new
                          {
                              Month = item.TransactionDate.Month,
                              Year = item.TransactionDate.Year
                          } into grp
                          orderby grp.Key.Year, grp.Key.Month
                          select new MonthlyCashFlow()
                          {
                              Month = grp.Key.Month,
                              Year = grp.Key.Year,
                              CreditAmount = grp.Sum(o => o.CreditAmount),
                              DebitAmount = grp.Sum(o => o.DebitAmount),
                          }).OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).Take(12).ToList();

            return result = result.OrderBy(o => o.Year).ThenBy(o => o.Month).ToList();
        }

        public Account Get(int id)
        {
            var result = base.Find<BankAccount>(id);
            return Mapper.Map<Account>(result);
        }

        public Account Add(Account account)
        {
            var bankAccount = Mapper.Map<BankAccount>(account);

            var result = base.Add(bankAccount);

            return Mapper.Map<Account>(result);
        }

        public Account Update(Account account)
        {
            var bankAccount = base.Find<BankAccount>(account.AccountID);
            Mapper.Map(account, bankAccount);

            var result = base.Update(bankAccount);

            return Mapper.Map<Account>(result);
        }

        public void Delete(int id)
        {
            base.Delete<BankAccount>(id);
        }

        public List<TagSummary> GetTags(int accountId)
        {
            var transactionsLst = base.Table<BankTransaction>()
                                        .Where(q => q.AccountId == accountId
                                            && q.TransactionDate <= DateTime.Now);

            var totalDebit = transactionsLst.Sum(s => s.DebitAmount);

            var result = (from item in transactionsLst
                          group item by new { Tag = item.Tag } into grp
                          select new TagSummary
                          {
                              Tag = grp.Key.Tag,
                              Amount = grp.Sum(s => s.DebitAmount),
                              Percentage = grp.Sum(s => s.DebitAmount) / totalDebit
                          }).OrderByDescending(o => o.Amount).ToList();

            foreach (var item in result)
                if (string.IsNullOrEmpty(item.Tag))
                    item.Tag = "None";

            return result;
        }
    }
}
