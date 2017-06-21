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
using BankA.Data.Helpers;

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

        public List<TagExpense> GetTop10Expenses(int accountId)
        {
            var transactionsLst = base.Table<BankTransaction>().Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId));

            var result = (from item in transactionsLst
                          group item by new { Tag = item.Tag } into grp
                          select new TagExpense
                          {
                              Tag = grp.Key.Tag,
                              Amount = grp.Sum(s => s.DebitAmount),
                          }).OrderByDescending(o => o.Amount).Take(10).ToList();

            return result.OrderBy(o => o.Amount).ToList();
        }

        public List<TagSummary> GetTagDetails(int accountId, int period)
        {
            var transactionsLst = base.Table<BankTransaction>().Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId));

            var debitTags = (from trans in transactionsLst
                             where trans.DebitAmount > 0
                             group trans by new
                             {
                                 Tag = trans.Tag,
                             } into grp
                             select new TagSummary
()
                             {
                                 Type = "D",
                                 Tag = grp.Key.Tag,
                                 Amount = grp.Sum(o => o.DebitAmount),
                                 Details = (from transDetail in transactionsLst
                                            where transDetail.Tag == grp.Key.Tag && transDetail.DebitAmount > 0
                                            orderby transDetail.TransactionDate.Year, transDetail.TransactionDate.Month
                                            group transDetail by new
                                            {
                                                Tag = transDetail.Tag,
                                                Month = transDetail.TransactionDate.Month,
                                                Year = transDetail.TransactionDate.Year
                                            } into grpDetail
                                            select new TagSummaryDetails()
                                            {
                                                Year = grpDetail.Key.Year,
                                                Month = grpDetail.Key.Month,
                                                Amount = grpDetail.Sum(o => o.DebitAmount),
                                            }).OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).Take(period).ToList()

                             });

            var creditTags = (from trans in transactionsLst
                             where trans.CreditAmount > 0
                             group trans by new
                             {
                                 Tag = trans.Tag,
                             } into grp
                             select new TagSummary()
                             {
                                 Type = "C",
                                 Tag = grp.Key.Tag,
                                 Amount = grp.Sum(o => o.CreditAmount),
                                 Details = (from transDetail in transactionsLst
                                            where transDetail.Tag == grp.Key.Tag && transDetail.CreditAmount > 0
                                            orderby transDetail.TransactionDate.Year, transDetail.TransactionDate.Month
                                            group transDetail by new
                                            {
                                                Tag = transDetail.Tag,
                                                Month = transDetail.TransactionDate.Month,
                                                Year = transDetail.TransactionDate.Year
                                            } into grpDetail
                                            select new TagSummaryDetails()
                                            {
                                                Year = grpDetail.Key.Year,
                                                Month = grpDetail.Key.Month,
                                                Amount = grpDetail.Sum(o => o.CreditAmount),
                                            }).OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).Take(period).ToList()

                             });

            //

            return debitTags.Union(creditTags).OrderByDescending(o => o.Amount).ToList();
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
                          }).OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).Take(period).ToList();

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
