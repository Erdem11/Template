using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Template.Data;
using Template.Entities.Abstract;
using Template.Entities.Concrete;
using Template.Service.Concrete;
using Template.Service.Helpers;

namespace Template.Service.Abstract
{
    public abstract class ServiceBase<T> where T : EntityBase
    {
        private DbSet<T> _dbSet;
        protected DbSet<T> DbSet => _dbSet ?? Context.Set<T>();
        
        internal readonly TemplateContext Context;
        protected ServiceBase(TemplateContext context)
        {
            Context = context;
        }

        public T Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();

            return entity;
        }

        public T Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();

            return entity;
        }

        public T Get(Guid id)
        {
            var result = Context.Set<T>().Find(id);

            return result;
        }
    }
}