using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Data;
using Template.Domain.Dto.Abstract;

namespace Template.Service
{
    public interface IServiceBase<T> where T : class, IEntityBase
    {
        public Task<T> AddAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task<T> DeleteAsync(T entity);
        public Task<T> GetAsync(Guid id);
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

        public async Task<T> AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> GetAsync(Guid id)
        {
            var result = await Context.Set<T>().FindAsync(id);

            return result;
        }
    }
}