using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Template.Caching
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string cacheKey);
    }

    public class ResponseCacheService : IResponseCacheService
    {
        private readonly ICacheService _cacheService;
        public ResponseCacheService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
            {
                return;
            }

            var serializedResponse = JsonConvert.SerializeObject(response);
            await _cacheService.SetAsync(cacheKey, serializedResponse, timeToLive);
        }
        
        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _cacheService.GetAsync<string>(cacheKey);
            return string.IsNullOrWhiteSpace(cachedResponse) ? null : cachedResponse;
        }
    }
}