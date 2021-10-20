using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Template.Entities.Abstract;
using Template.Service.Abstract;

namespace Template.Service.Helpers
{
    public static class PagingExtensions
    {
        public static List<T> GetWithCount<T>(this ServiceBase<T> service,
            int pageNo,
            int pageSize,
            out int totalCount,
            Expression<Func<T, bool>> predicate)
            where T : EntityBase
        {
            var query = service.Context.Set<T>().Where(predicate);

            return GetWithCountQueryExecute(pageNo, pageSize, out totalCount, query);
        }

        internal static List<T> GetWithCount<T>(this DbSet<T> dbSet,
            int pageNo,
            int pageSize,
            out int totalCount,
            Expression<Func<T, bool>> predicate)
            where T : EntityBase
        {
            var query = dbSet.Where(predicate);

            return GetWithCountQueryExecute(pageNo, pageSize, out totalCount, query);
        }

        internal static List<T> GetWithCount<T>(this IQueryable<T> queryable,
            int pageNo,
            int pageSize,
            out int totalCount,
            Expression<Func<T, bool>> predicate)
            where T : EntityBase
        {
            var query = queryable.Where(predicate);

            return GetWithCountQueryExecute(pageNo, pageSize, out totalCount, query);
        }

        private static List<T> GetWithCountQueryExecute<T>(int pageNo, int pageSize, out int totalCount, IQueryable<T> query) where T : EntityBase
        {
            var results = query
                .Select(p => new
                {
                    Entity = p,
                    TotalCount = query.Count()
                })
                .Skip(pageNo * pageSize).Take(pageSize)
                .ToList();// one hit query

            var resultsFirst = results.FirstOrDefault();
            if (resultsFirst == default)
            {
                totalCount = 0;
                return new List<T>();
            }

            totalCount = resultsFirst.TotalCount;
            var entities = results.Select(r => r.Entity).ToList();

            return entities;
        }
    }
}