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
using BankA.Models.Tags;

namespace BankA.Data.Repositories
{
    public class TagRepository : Repository
    {

        public TagRepository(string currentUser):base(currentUser)
        {
        }

        public List<TagInfo> Get()
        {
            var result = base.Table<BankTag>().ProjectTo<TagInfo>().ToList();
            return result;
        }

        public List<string> GetLookup()
        {
            var result = base.Table<BankTag>().Select(s => s.Tag).ToList();
            return result;
        }

        public TagInfo Get(int id)
        {
            var result = base.Find<BankTag>(id);
            return Mapper.Map<TagInfo>(result);
        }

        public TagInfo Add(TagInfo tag)
        {
            ValidateTag(tag);

            var bankTag = Mapper.Map<BankTag>(tag);

            var result = base.Add(bankTag);

            ApplyToTransactions(tag);

            return Mapper.Map<TagInfo>(result);
        }

        public TagInfo Update(TagInfo tag)
        {
            var bankTag = base.Find<BankTag>(tag.TagId);
            Mapper.Map(tag, bankTag);

            var result = base.Update(bankTag);

            return Mapper.Map<TagInfo>(result);
        }

        public void Delete(int id)
        {
            base.Delete<BankTag>(id);
        }

        private void ApplyToTransactions(TagInfo tag)
        {
            var transactions = base.Table<BankTransaction>()
                                    .Where(s => string.IsNullOrEmpty(s.Tag)
                                        && s.Description.ToUpper().Contains(tag.Description.ToUpper()))
                                    .ToList();

            foreach (var transaction in transactions)
            {
                transaction.Tag = tag.Tag;
                base.Update(transaction);
            }
        }

        private void ValidateTag(TagInfo tag)
        {
            var result = base.Table<BankTag>().Where(q => q.Description.ToLower() == tag.Description.ToLower()).FirstOrDefault();
            if (result != null)
                throw new Exception($"'{tag.Description}' already has tag '{result.Tag}'");
        }
    }
}
