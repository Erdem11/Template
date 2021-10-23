using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Template.Common.Structs;
using Template.Entities.Abstract;

namespace Template.Service.Helpers
{
    public static class SoftDeleteServiceExtensions
    {
        public static IQueryable<T> ExcludeDeleted<T>(this IQueryable<T> query, bool exclude = true) where T : ISoftDelete
        {
            return query.Where(x => !exclude || !x.IsDeleted);
        }

        public static T SoftDelete<T>(this ServiceBase<T> service, T entity) where T : class, IEntityBase, ISoftDelete 
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            service.Context.Entry(entity).State = EntityState.Modified;
            service.Context.SaveChanges();

            return entity;
        }

        public static T SoftDelete<T>(this ServiceBase<T> service, MyKey id) where T : class, IEntityBase, ISoftDelete 
        {
            var entity = service.Context.Set<T>().Find(id);

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            service.Context.Entry(entity).State = EntityState.Modified;
            service.Context.SaveChanges();

            return entity;
        }
    }
}