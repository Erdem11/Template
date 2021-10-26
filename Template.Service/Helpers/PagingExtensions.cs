using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Template.Common.Structs;
using Template.Domain.Dto.Abstract;

namespace Template.Service.Helpers
{
    public static class PagingExtensions
    {
        public static List<T> GetWithCount<T>(this ServiceBase<T> service,
            PaginationFilter paginationFilter,
            Expression<Func<T, bool>> predicate)
            where T : class, IEntityBase
        {
            var query = service.Context.Set<T>().Where(predicate);

            return GetWithCountQueryExecute(paginationFilter, query);
        }

        internal static List<T> GetWithCount<T>(this DbSet<T> dbSet,
            PaginationFilter paginationFilter,
            Expression<Func<T, bool>> predicate)
            where T : class, IEntityBase
        {
            var query = dbSet.Where(predicate);

            return GetWithCountQueryExecute(paginationFilter, query);
        }

        internal static List<T> GetWithCount<T>(this IQueryable<T> queryable,
            PaginationFilter paginationFilter,
            Expression<Func<T, bool>> predicate)
            where T : class, IEntityBase
        {
            var query = queryable.Where(predicate);

            return GetWithCountQueryExecute(paginationFilter, query);
        }

        private static List<T> GetWithCountQueryExecute<T>(PaginationFilter paginationFilter, IQueryable<T> queryable) where T : class, IEntityBase
        {
            var results = queryable
                .Select(p => new
                {
                    Entity = p,
                    TotalCount = queryable.Count()
                })
                .Skip(paginationFilter.PageNo * paginationFilter.PageSize).Take(paginationFilter.PageSize)
                .ToList();// one hit query

            var resultsFirst = results.FirstOrDefault();
            if (resultsFirst == default)
            {
                paginationFilter.TotalCount = 0;
                return new List<T>();
            }

            paginationFilter.TotalCount = resultsFirst.TotalCount;
            var entities = results.Select(r => r.Entity).ToList();

            return entities;
        }

        public static IQueryable<T> GetPagedQuery<T>(this IQueryable<T> queryable, PaginationFilter paginationFilter)
        {
            return queryable.Skip(paginationFilter.PageSize * paginationFilter.PageNo).Take(paginationFilter.PageSize);
        }
    }
}