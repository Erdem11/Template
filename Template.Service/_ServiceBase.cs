using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Data;
using Template.Domain.Dto.Abstract;

namespace Template.Service
{
    public interface IServiceBase<T> where T : class, IEntityBase
    {
        public Task<T> AddAsync(T entity, bool saveChanges = false);
        public Task<T> UpdateAsync(T entity, bool saveChanges = false);
        public Task<T> DeleteAsync(T entity, bool saveChanges = false);
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

        public async Task<T> AddAsync(T entity, bool saveChanges = false)
        {
            await Context.Set<T>().AddAsync(entity);

            if (saveChanges)
                await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<T>> AddAsync(List<T> entities, bool saveChanges = false)
        {
            await Context.Set<T>().AddRangeAsync(entities);

            if (saveChanges)
                await Context.SaveChangesAsync();

            return entities;
        }

        public async Task<T> UpdateAsync(T entity, bool saveChanges = false)
        {
            Context.Entry(entity).State = EntityState.Modified;

            if (saveChanges)
                await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<T>> UpdateAsync(List<T> entities, bool saveChanges = false)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            if (saveChanges)
                await Context.SaveChangesAsync();

            return entities;
        }

        public async Task<T> DeleteAsync(T entity, bool saveChanges = false)
        {
            Context.Set<T>().Remove(entity);

            if (saveChanges)
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