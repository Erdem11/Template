using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using Template.Data;

namespace Template.Api.Hubs
{
    public class MessagingHubHelper
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly MessagingContext _messagingContext;
        public MessagingHubHelper(IConnectionMultiplexer connectionMultiplexer, MessagingContext messagingContext)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _messagingContext = messagingContext;

        }

        public async Task AddMessageToDb(Guid userId, string message)
        {
            _messagingContext.Messages.Add(new Message
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Text = message
            });

            await _messagingContext.SaveChangesAsync();
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