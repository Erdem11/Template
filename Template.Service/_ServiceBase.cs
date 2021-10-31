using System;
using Microsoft.EntityFrameworkCore;
using Template.Common.Structs;
using Template.Data;
using Template.Domain.Dto.Abstract;

namespace Template.Service
{
    public interface IServiceBase<T> where T : class, IEntityBase
    {
        public T Add(T entity);
        public T Update(T entity);
        public T Delete(T entity);
        public T Get(Guid id);
    }

    public abstract class ServiceBase<T> : IServiceBase<T> where T : class, IEntityBase
    {
        internal readonly TemplateContext Context;
        private DbSet<T> _dbSet;
        protected ServiceBase(TemplateContext context)
        {
            Context = context;
        }
        protected DbSet<T> DbSet => _dbSet ??= Context.Set<T>();

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