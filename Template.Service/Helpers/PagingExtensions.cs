using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Template.Common.Structs;
using Template.Entities.Abstract;

namespace Template.Service.Helpers
{
    public static class PagingExtensions
    {
        public static List<T> GetWithCount<T>(this ServiceBase<T> service,
            PageHolder pageHolder,
            Expression<Func<T, bool>> predicate)
            where T : class, IEntityBase 
        {
            var query = service.Context.Set<T>().Where(predicate);

            return GetWithCountQueryExecute(pageHolder, query);
        }

        internal static List<T> GetWithCount<T>(this DbSet<T> dbSet,
            PageHolder pageHolder,
            Expression<Func<T, bool>> predicate)
            where T : class, IEntityBase 
        {
            var query = dbSet.Where(predicate);

            return GetWithCountQueryExecute(pageHolder, query);
        }

        internal static List<T> GetWithCount<T>(this IQueryable<T> queryable,
            PageHolder pageHolder,
            Expression<Func<T, bool>> predicate)
            where T : class, IEntityBase 
        {
            var query = queryable.Where(predicate);

            return GetWithCountQueryExecute(pageHolder, query);
        }

        private static List<T> GetWithCountQueryExecute<T>(PageHolder pageHolder, IQueryable<T> query) where T : class, IEntityBase 
        {
            var results = query
                .Select(p => new
                {
                    Entity = p,
                    TotalCount = query.Count()
                })
                .Skip(pageHolder.PageNo * pageHolder.PageSize).Take(pageHolder.PageSize)
                .ToList();// one hit query

            var resultsFirst = results.FirstOrDefault();
            if (resultsFirst == default)
            {
                pageHolder.TotalCount = 0;
                return new List<T>();
            }

            pageHolder.TotalCount = resultsFirst.TotalCount;
            var entities = results.Select(r => r.Entity).ToList();

            return entities;
        }
    }
}