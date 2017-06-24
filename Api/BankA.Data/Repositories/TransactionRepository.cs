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
using System.Linq.Expressions;
using BankA.Models.Accounts;

namespace BankA.Data.Repositories
{
    public class TransactionRepository : Repository
    {

        public TransactionRepository(string currentUser):base(currentUser)
        {
        }

        public TransationSearchResult GetTransactions(int accountId, TransactionSearch search)
        {
            if (search.Query == null)
                search.Query = string.Empty;

            int pageSize = 20;
            var transactions = base.Table<BankTransaction>()
                        .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId)
                                && (q.Description.Contains(search.Query) || q.Tag.Contains(search.Query)))
                        .OrderByDescending(o => o.TransactionDate)
                        .ProjectTo<Transaction>().ToList();

            var result = new TransationSearchResult()
            {
                ItemsPerPage = pageSize,
                Page = search.Page,
                Count = transactions.Count,
                Data = transactions.Skip((search.Page - 1) * pageSize).Take(pageSize).ToList()
            };

            return result;
        }

       

        public List<TagExpense> GetTop10Expenses(int accountId, int period)
        {
            var transactionsLst = base.Table<BankTransaction>().Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId)
                                                                    && q.IsTransfer == false
                                                                    && q.DebitAmount > 0
                                                                    && q.TransactionDate >= DateTimeHelper.StartPriod(period));

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
            var transactionsLst = base.Table<BankTransaction>().Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId)
                                                                    && q.IsTransfer == false);

            var debitTags = (from trans in transactionsLst
                             where trans.DebitAmount > 0
                             group trans by new
                             {
                                 Tag = trans.Tag,
                             } into grp
                             select new TagSummary()
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

        private List<BalanceView> GetBalance(int accountId, int period)
        {
            var transactionsLst = base.Table<BankTransaction>()
                .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId));

            var result = (from item in transactionsLst
                          group item by new
                          {
                              Month = item.TransactionDate.Month,
                              Year = item.TransactionDate.Year
                          } into grp
                          orderby grp.Key.Year, grp.Key.Month
                          select new BalanceView
                          {
                              Date = $"{grp.Key.Month}/{grp.Key.Year}",
                              Balance = grp.Sum(sum => sum.CreditAmount - sum.DebitAmount),
                          }).OrderByDescending(o => o.Date).Take(period).ToList();

            return result = result.OrderBy(o => o.Date).ToList(); ;
        }


        public AccountSummary GetAccountSummary(int accountId, int period)
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

            var transactions = GetTransactionView(accountId).Where(q => q.TransactionDate >= DateTimeHelper.StartPriod(period));

            //var result1 = (from item in transactionsLst
            //               where item.TransactionDate >= DateTimeHelper.StartPriod(period)
            //               group item by item into grp
            //               select new
            //               {
            //                   DebitAmount = grp.Sum(o => o.DebitAmount),
            //                   TransferOutAmount = grp.Sum(o => o.TransferOutAmount),
            //                   CreditAmount = grp.Sum(o => o.CreditAmount),
            //                   TransferInAmount = grp.Sum(o => o.TransferInAmount),
            //               }).FirstOrDefault();

            result.CreditAmount = (decimal?)transactions.Sum(sum => sum.CreditAmount) ?? 0;
            result.TransferInAmount = (decimal?)transactions.Sum(sum => sum.TransferInAmount) ?? 0;

            result.DebitAmount = (decimal?)transactions.Sum(sum => sum.DebitAmount) ?? 0;
            result.TransferOutAmount = (decimal?)transactions.Sum(sum => sum.TransferOutAmount) ?? 0;
            //  //result.Balance = GetBalance(accountId);
            result.Balance = (result.CreditAmount + result.TransferInAmount) - (result.DebitAmount + result.TransferOutAmount);
            result.FirstTransactionDate = (DateTime?)transactions.Min(m => m.TransactionDate) ?? DateTime.MinValue;
            result.LastTransactionDate = (DateTime?)transactions.Max(m => m.TransactionDate) ?? DateTime.MinValue;


            //var transactions = base.Table<BankTransaction>().Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId));

            //  result.CreditAmount = (decimal?)transactions.Where(q => q.TransactionDate >= DateTimeHelper.StartPriod(period) && q.IsTransfer == false).Sum(sum => sum.CreditAmount) ?? 0;
            //  result.TransferInAmount = (decimal?)transactions.Where(q => q.TransactionDate >= DateTimeHelper.StartPriod(period) && q.IsTransfer == true).Sum(sum => sum.CreditAmount) ?? 0;

            //  result.DebitAmount = (decimal?)transactions.Where(q => q.TransactionDate >= DateTimeHelper.StartPriod(period) && q.IsTransfer == false).Sum(sum => sum.DebitAmount) ?? 0;
            //  result.TransferOutAmount = (decimal?)transactions.Where(q => q.TransactionDate >= DateTimeHelper.StartPriod(period) && q.IsTransfer == true).Sum(sum => sum.DebitAmount) ?? 0;
            //  //result.Balance = GetBalance(accountId);
            //  result.Balance = (result.CreditAmount + result.TransferInAmount) - (result.DebitAmount + result.TransferOutAmount);
            //  result.FirstTransactionDate = (DateTime?)transactions.Min(m => m.TransactionDate) ?? DateTime.MinValue;
            //  result.LastTransactionDate = (DateTime?)transactions.Max(m => m.TransactionDate) ?? DateTime.MinValue;

            return result;
        }

        public List<MonthlyCashFlow> GetMonthlyCashFlow(int accountId, int period)
        {

            var transactionsLst = GetTransactionView(accountId);

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
                              TransferOutAmount = grp.Sum(o => o.TransferOutAmount),
                              CreditAmount = grp.Sum(o => o.CreditAmount),
                              TransferInAmount = grp.Sum(o => o.TransferInAmount),
                          }).ToList()
                          .Select(s =>
                          {
                              balance += ((s.CreditAmount + s.TransferInAmount) - (s.DebitAmount + s.TransferOutAmount));
                              return new MonthlyCashFlow()
                              {
                                  Month = s.Month,
                                  Year = s.Year,
                                  CreditAmount = s.CreditAmount,
                                  TransferInAmount = s.TransferInAmount,
                                  DebitAmount = s.DebitAmount,
                                  TransferOutAmount = s.TransferOutAmount,
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

        public void MarkAsTransfer(int transactionId, bool isTransfer)
        {
            var transaction = base.Find<BankTransaction>(transactionId);
            transaction.IsTransfer = isTransfer;
            base.Update(transaction);
        }


        public IEnumerable<TransactionView> GetTransactionView(int accountId)
        {

            var transactionsLst = base.Table<BankTransaction>()
                                        .Where(q => q.AccountId == (accountId > 0 ? accountId : q.AccountId));

            if (accountId > 0)
            {
                var result = (from item in transactionsLst
                              where item.IsTransfer == false
                              select new TransactionView
                              {
                                  AccountId = item.AccountId,
                                  TransactionDate = item.TransactionDate,
                                  CreditAmount = item.CreditAmount,
                                  DebitAmount = item.DebitAmount,
                              })
                         .Union
                         (from item in transactionsLst
                          where item.IsTransfer == true
                          select new TransactionView
                          {
                              AccountId = item.AccountId,
                              TransactionDate = item.TransactionDate,
                              TransferInAmount = item.CreditAmount,
                              TransferOutAmount = item.DebitAmount,
                          }
                       ).ToList();

                return result;
            }
            else
            {
                var result = (from item in transactionsLst
                              where item.IsTransfer == false
                              select new TransactionView
                              {
                                  AccountId = item.AccountId,
                                  TransactionDate = item.TransactionDate,
                                  CreditAmount = item.CreditAmount,
                                  DebitAmount = item.DebitAmount,
                              }).ToList();

                return result;
            }

           




        }

    }
}
