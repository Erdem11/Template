using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Dto.Abstract;

namespace Template.Service.Helpers
{
    public static class SoftDeleteServiceExtensions
    {
        public static IQueryable<T> ExcludeDeleted<T>(this IQueryable<T> query, bool exclude = true) where T : ISoftDelete
        {
            return query.Where(x => !exclude || !x.IsDeleted);
        }

        public static async Task<T> SoftDeleteAsync<T>(this ServiceBase<T> service, T entity, bool saveChanges = false) where T : class, IEntityBase, ISoftDelete
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            service.Context.Entry(entity).State = EntityState.Modified;

            if (saveChanges)
                await service.Context.SaveChangesAsync();

            return entity;
        }

        public static async Task<T> SoftDeleteAsync<T>(this ServiceBase<T> service, Guid id, bool saveChanges = false) where T : class, IEntityBase, ISoftDelete
        {
            var entity = await service.Context.Set<T>().FindAsync(id);

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            service.Context.Entry(entity).State = EntityState.Modified;

            if (saveChanges)
                await service.Context.SaveChangesAsync();

            return entity;
        }

        public static async Task<List<T>> SoftDeleteAsync<T>(this ServiceBase<T> service, List<Guid> idList, bool saveChanges = false) where T : class, IEntityBase, ISoftDelete
        {
            var entities = await service.Context.Set<T>().Where(x => idList.Contains(x.Id)).ToListAsync();

            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                service.Context.Entry(entity).State = EntityState.Modified;
            }

            if (saveChanges)
                await service.Context.SaveChangesAsync();

            return entities;
        }

        public static async Task<List<T>> SoftDeleteAsync<T>(this ServiceBase<T> service, List<T> entities, bool saveChanges = false) where T : class, IEntityBase, ISoftDelete
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;

                service.Context.Entry(entity).State = EntityState.Modified;
            }

            if (saveChanges)
                await service.Context.SaveChangesAsync();

            return entities;
        }
    }
}