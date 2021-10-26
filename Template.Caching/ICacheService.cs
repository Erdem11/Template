using System;
using System.Threading.Tasks;

namespace Template.Caching
{
    public interface ICacheService
    {
        Task<string> GetCacheValueAsync(string key);
        Task SetCacheValueAsync(string key, string value, TimeSpan? expire = default);
    }
}