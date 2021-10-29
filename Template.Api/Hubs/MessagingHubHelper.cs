using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Template.Api.Hubs
{
    public class MessagingHubHelper
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public MessagingHubHelper(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;

        }
        public async Task AddToRedis(HubUser user)
        {
            await _connectionMultiplexer.GetDatabase().StringSetAsync($"MessagingHub:Users:{user.ConnectionId}", JsonConvert.SerializeObject(user));
        }

        public async Task RemoveFromRedis(string connectionId)
        {
            await _connectionMultiplexer.GetDatabase().KeyDeleteAsync($"MessagingHub:Users:{connectionId}");
        }
    }
}