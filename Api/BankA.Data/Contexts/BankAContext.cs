using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.PlatformAbstractions;
using BankA.Data.SeedData;
using BankA.Data.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Text;

namespace BankA.Data.Contexts
{


    internal class BankAContext : DbContext
    {
        public virtual DbSet<BankAccount> BankAccount { get; set; }
        public virtual DbSet<BankFile> BankFile { get; set; }
        public virtual DbSet<BankTransaction> BankTransaction { get; set; }
        public virtual DbSet<BankTag> BankTag { get; set; }
        public virtual DbSet<BankVersion> BankVersion { get; set; }

        private string currentUser;

        public BankAContext(string currentUser)
        {
            this.currentUser = currentUser;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
#if DEBUG
            optionsBuilder.UseSqlServer(@"Server=.\SqlExpress;Database=BankA_Dev1;Integrated Security=True;");
#else
            optionsBuilder.UseSqlite("Data Source=banka.db");
#endif

        }

        public override int SaveChanges()
        {
            try
            {
                Auditing();

                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                var message = GetExceptionMessages(ex);
                throw new Exception(message, ex);
            }
        }

        private string GetExceptionMessages(Exception ex)
        {
            var s = new StringBuilder("An Exception was caught while saving changes. ");
            Exception e = ex;
            while (e != null)
            {
                s.AppendLine(e.Message);
                e = e.InnerException;
            }
            return s.ToString();

        }

        private Exception HandleDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }

            string message = builder.ToString();
            return new Exception(message, dbu);
        }

        private void Auditing()
        {
            var modifiedEntries = ChangeTracker.Entries<IEntityBase>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (EntityEntry<IEntityBase> entry in modifiedEntries)
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ChangedBy = currentUser;
                    entry.Entity.ChangedOn = DateTime.Now;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = currentUser;
                    entry.Entity.CreatedOn = DateTime.Now;
                }
            }
        }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId);
                entity.Property(e => e.AccountId);
                entity.Property(e => e.ChangedBy).HasMaxLength(50);
                entity.Property(e => e.ChangedOn);
                entity.Property(e => e.Closed);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedOn);
                entity.Property(e => e.Description).IsRequired();

                entity.HasMany(d => d.Transactions)
                   .WithOne(p => p.Account)
                   .HasForeignKey(d => d.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BankFile>(entity =>
            {
                entity.HasKey(e => e.FileId);
                entity.Property(e => e.FileId);
                entity.Property(e => e.AccountId);
                entity.Property(e => e.ChangedBy).HasMaxLength(50);
                entity.Property(e => e.ChangedOn);
                entity.Property(e => e.ContentType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedOn);
                entity.Property(e => e.FileContent);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(50);

                entity.HasMany(s => s.Transactions)
                    .WithOne(s => s.File)
                    .HasForeignKey(s => s.FileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BankTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId);
                entity.Property(e => e.AccountId);
                entity.Property(e => e.ChangedBy).HasMaxLength(50);
                entity.Property(e => e.ChangedOn);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedOn);
                entity.Property(e => e.CreditAmount);
                entity.Property(e => e.DebitAmount);
                entity.Property(e => e.Description);
                entity.Property(e => e.FileId);
                entity.Property(e => e.Tag).HasMaxLength(50);
                entity.Property(e => e.TransactionDate);
                entity.Property(e => e.TransactionType).HasMaxLength(50);

            });

            modelBuilder.Entity<BankTag>(entity =>
            {
                entity.HasKey(e => e.TagId);
                entity.Property(e => e.TagId);
                entity.Property(e => e.ChangedBy).HasMaxLength(50);
                entity.Property(e => e.ChangedOn);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedOn);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Tag).HasMaxLength(50);
            });

            modelBuilder.Entity<BankVersion>(entity =>
            {
                entity.HasKey(e => e.Version);
                entity.Property(e => e.Version).HasMaxLength(50);
            });
        }
    }
}
