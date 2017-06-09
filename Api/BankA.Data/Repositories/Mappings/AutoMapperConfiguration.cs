using AutoMapper;
using BankA.Data.Models;
using BankA.Models.Accounts;
using BankA.Models.Tags;
using BankA.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankA.Data.Repositories.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.CreateMap<BankAccount, Account>();
                cfg.CreateMap<BankTransaction, Transaction>();
                cfg.CreateMap<BankTag, TagInfo>();

                cfg.CreateMap<BankTag, String>().ConvertUsing(source => source.Tag ?? string.Empty);
            });
        }
    }

}
