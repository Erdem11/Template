using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Template.Caching.InMemoryCaching
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(_cache.Get<T>(key));
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expire = default)
        {
            if (expire != default)
            {
                _cache.Set(key, value, expire.Value);
                return Task.CompletedTask;
            }

            _cache.Set(key, value);
            return Task.CompletedTask;
        }

        public async Task SetSeparateAsync<T>(string keyPrefix, List<T> values, Expression<Func<T, string>> nameOfProperty, TimeSpan? expire = default)
        {
            var name = ((MemberExpression)nameOfProperty.Body).Member.Name;

            await Task.Run(() => {
                Parallel.ForEach(values, value => {

                    if (expire != default)
                    {
                        _cache.Set(keyPrefix + name, value, expire.Value);
                        return;
                    }

                    _cache.Set(keyPrefix + name, value);
                });
            });
        }

        public Task DeleteAsync(string key)
        {
            _cache.Remove(key);

            return Task.CompletedTask;
        }
    }
}