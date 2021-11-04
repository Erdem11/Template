using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Template.Caching.RedisCaching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var stringValue = await db.StringGetAsync(key);

            if (stringValue == default)
            {
                return default;
            }

            var value = JsonConvert.DeserializeObject<T>(stringValue);

            return value;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expire = default)
        {
            var db = _connectionMultiplexer.GetDatabase();

            var objectValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await db.StringSetAsync(key, objectValue, expire);
        }

        public async Task SetSeparateAsync<T>(string keyPrefix, List<T> values, Expression<Func<T, string>> nameOfProperty, TimeSpan? expire = default)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var name = ((MemberExpression)nameOfProperty.Body).Member.Name;

            await Task.Run(() => {
                Parallel.ForEach(values, async value => {
                    var objectValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    await db.StringSetAsync(keyPrefix + name, objectValue, expire);
                });
            });
        }

        public async Task DeleteAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}