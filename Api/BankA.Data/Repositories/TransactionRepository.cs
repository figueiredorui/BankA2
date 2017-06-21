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
using BankA.Models.Reports;
using BankA.Models.Tags;
using System.Globalization;

namespace BankA.Data.Repositories
{
    public class TransactionRepository : Repository
    {

        public TransactionRepository(string currentUser):base(currentUser)
        {
        }

        public TransactionResult GetTransactions(int accountId, TransactionSearch search)
        {
            if (search.Query == null)
                search.Query = string.Empty;

            int pageSize = 20;
            var transactions = base.Table<BankTransaction>()
                        .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId)
                                && (q.Description.Contains(search.Query) || q.Tag.Contains(search.Query)))
                        .OrderByDescending(o => o.TransactionDate)
                        .ProjectTo<Transaction>().ToList();

            var result = new TransactionResult()
            {
                ItemsPerPage = pageSize,
                Page = search.Page,
                Count = transactions.Count,
                Data = transactions.Skip((search.Page - 1) * pageSize).Take(pageSize).ToList()
            };

            return result;
        }

        private decimal GetBalance(int accountId, DateTime date)
        {
            var transactionsLst = base.Table<BankTransaction>()
                .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId)
                && q.TransactionDate < date);

            var balance = (decimal?)transactionsLst.Sum(sum => sum.CreditAmount - sum.DebitAmount) ?? 0;

            return balance;
        }

        //public List<TagSummary> GetTags(int accountId)
        //{
        //    var transactionsLst = base.Table<BankTransaction>()
        //                                .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId)
        //                                    && q.TransactionDate <= DateTime.Now);

        //    var totalDebit = transactionsLst.Sum(s => s.DebitAmount);

        //    var result = (from item in transactionsLst
        //                  group item by new { Tag = item.Tag } into grp
        //                  select new TagSummary
        //                  {
        //                      Tag = grp.Key.Tag,
        //                      Amount = grp.Sum(s => s.DebitAmount),
        //                      Percentage = grp.Sum(s => s.DebitAmount) / totalDebit
        //                  }).OrderByDescending(o => o.Amount).ToList();

        //    foreach (var item in result)
        //        if (string.IsNullOrEmpty(item.Tag))
        //            item.Tag = "None";

        //    return result;
        //}



        //public List<ExpensesReport> GetExpenses(int accountId, DateTime startDate, DateTime endDate)
        //{
        //    var transactionsLst = base.Table<BankTransaction>()
        //                                                .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId)
        //                                                    && q.TransactionDate >= startDate
        //                                                    && q.TransactionDate <= endDate
        //                                                    && q.DebitAmount > 0);

        //    var lst = (from trans in transactionsLst
        //               group trans by new
        //               {
        //                   Tag = trans.Tag,
        //                   Month = trans.TransactionDate.Month,
        //                   Year = trans.TransactionDate.Year
        //               } into grp
        //               select new ExpensesReport()
        //               {
        //                   Tag = grp.Key.Tag,
        //                   Year = grp.Key.Year,
        //                   Month = grp.Key.Month,
        //                   Amount = grp.Sum(o => o.DebitAmount),
        //               }).ToList();

        //    return lst;
        //}

        private DateTime StartPriod(int period)
        {
            var date = DateTime.Now.AddMonths(-period);
            return new DateTime(date.Year, date.Month, 1);
        }

        public List<TagExpenses> GetTagExpenses(int accountId, int period)
        {
            var transactionsLst = base.Table<BankTransaction>()
                                                       .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId)
                                                            && q.TransactionDate >= StartPriod(period)
                                                            && q.DebitAmount > 0);

            var lst = (from trans in transactionsLst
                       group trans by new
                       {
                           Tag = trans.Tag,
                       } into grp
                       select new TagExpenses()
                       {
                           Tag = grp.Key.Tag,
                           Amount = grp.Sum(o => o.DebitAmount),
                           Details = (from transDetail in transactionsLst
                                      where transDetail.Tag == grp.Key.Tag
                                      
                                      orderby transDetail.TransactionDate.Year, transDetail.TransactionDate.Month
                                      group transDetail by new
                                      {
                                          Tag = transDetail.Tag,
                                          Month = transDetail.TransactionDate.Month,
                                          Year = transDetail.TransactionDate.Year
                                      } into grpDetail
                                      select new TagExpensesDetails()
                                      {
                                          //Tag = grpDetail.Key.Tag,
                                          Year = grpDetail.Key.Year,
                                          Month = grpDetail.Key.Month,
                                          Amount = grpDetail.Sum(o => o.DebitAmount),
                                      }).ToList()

                       }).OrderByDescending(o => o.Amount).ToList();

            return lst;
        }



        public List<MonthlyCashFlow> GetMonthlyCashFlow(int accountId, int period)
        {
            var transactionsLst = base.Table<BankTransaction>()
                                        .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId)
                                           // && q.IsTransfer == false
                                           //  && q.TransactionDate >= StartPriod(period)
                                           );

            decimal balance = 0;
            var result = (from item in transactionsLst
                          group item by new
                          {
                              Month = item.TransactionDate.Month,
                              Year = item.TransactionDate.Year
                          } into grp
                          orderby grp.Key.Year, grp.Key.Month
                          select new
                          {
                              Month = grp.Key.Month,
                              Year = grp.Key.Year,
                              DebitAmount = grp.Sum(o => o.DebitAmount),
                              CreditAmount = grp.Sum(o => o.CreditAmount),
                          }).ToList()
                          .Select(s =>
                          {
                              balance += s.CreditAmount - s.DebitAmount;
                              return new MonthlyCashFlow()
                              {
                                  Month = s.Month,
                                  Year = s.Year,
                                  CreditAmount = s.CreditAmount,
                                  DebitAmount = s.DebitAmount,
                                  Balance = balance
                              };
                          }).OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).Take(12).ToList();

            return result = result.OrderBy(o => o.Year).ThenBy(o => o.Month).ToList();
        }

        public void UpdateTag(int transactionId, string tag)
        {
            var transaction = base.Find<BankTransaction>(transactionId);
            transaction.Tag = tag;
            base.Update(transaction);
        }


    }
}
