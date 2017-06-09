using BankA.Data.Contexts;
using BankA.Data.Models;
using BankA.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankA.Data.Repositories
{
    public abstract class Repository
    {
        private readonly string currentUser;
        private readonly BankAContext ctx;

        public Repository(string currentUser)
        {
            this.currentUser = currentUser;
            ctx = new BankAContext(currentUser);
        }

        protected IQueryable<TEntity> Table<TEntity>() where TEntity : class
        {
            return ctx.Set<TEntity>().AsNoTracking();
        }

        protected TEntity Find<TEntity>(object id) where TEntity : class
        {
            var entity = ctx.Set<TEntity>().Find(id);
            if (entity == null)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found");

            return entity;
        }

        protected TEntity Add<TEntity>(TEntity entity) where TEntity : class, IEntityBase
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ctx.Set<TEntity>().Add(entity);

            entity.CreatedBy = currentUser;
            entity.CreatedOn = DateTime.Now;

            this.ctx.SaveChanges();

            return entity;
        }

        protected TEntity Update<TEntity>(TEntity entity) where TEntity : class, IEntityBase
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (ctx.Entry<TEntity>(entity).State == EntityState.Detached)
                ctx.Set<TEntity>().Update(entity);

            entity.ChangedBy = currentUser;
            entity.ChangedOn = DateTime.Now;

            this.ctx.SaveChanges();

            return entity;
        }

        protected void Delete<TEntity>(object id) where TEntity : class
        {
            var entity = this.Find<TEntity>(id);
            this.Delete(entity);
        }

        protected void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ctx.Set<TEntity>().Remove(entity);

            this.ctx.SaveChanges();
        }

        protected int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return ctx.Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}
