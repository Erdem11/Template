using System;
using Microsoft.EntityFrameworkCore;
using Template.Data;
using Template.Entities.Abstract;

namespace Template.Service
{
    public interface IServiceBase<T> where T : EntityBase
    {
        public T Add(T entity);
        public T Update(T entity);
        public T Delete(T entity);
        public T Get(Guid id);
    }
    
    public abstract class ServiceBase<T> : IServiceBase<T> where T : EntityBase
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