using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Template.Caching
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expire = default);
        Task SetSeparateAsync<T>(string keyPrefix, List<T> values, Expression<Func<T, string>> nameOfProperty, TimeSpan? expire = default);
        Task DeleteAsync(string key);
    }
}