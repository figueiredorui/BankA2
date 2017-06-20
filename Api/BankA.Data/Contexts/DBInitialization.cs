using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BankA.Data.Contexts
{
    public class DBInitialization
    {
        public static void Initialize()
        {
            using (var context = new BankAContext(""))
            {
                context.Database.EnsureCreated();
             //   context.Database.Migrate();

               BankA.Data.SeedData.SeedDataHelper.EnsureSeedData(context);
            }
        }
    }
}
