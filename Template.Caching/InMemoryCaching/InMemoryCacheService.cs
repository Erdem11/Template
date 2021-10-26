using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Template.Caching.InMemoryCaching
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public Task<string> GetCacheValueAsync(string key)
        {
            return Task.FromResult(_cache.Get<string>(key));
        }

        public Task SetCacheValueAsync(string key, string value, TimeSpan? expire = default)
        {
            if (expire != default)
            {
                _cache.Set(key, value, expire.Value);
                return Task.CompletedTask;
            }
            
            _cache.Set(key, value);
            return Task.CompletedTask;
        }
    }
}