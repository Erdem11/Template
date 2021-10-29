using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StackExchange.Redis;
using Template.Common.SettingsConfigurationFiles;
using Template.Data;

namespace Template.Api.Hubs
{
    public class MessagingHub : Hub<IMessagingHub>
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly SettingsHolder _settingsHolder;
        private readonly MessagingContext _messagingContext;
        
        public MessagingHub(IConnectionMultiplexer connectionMultiplexer, SettingsHolder settingsHolder, MessagingContext messagingContext)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _settingsHolder = settingsHolder;
            _messagingContext = messagingContext;
        }

        public override Task OnConnectedAsync()
        {
            var user = new HubUser();
            user.UserName = Context.GetHttpContext().Request.Query["userName"].ToString();
            user.ConnectionId = Context.ConnectionId;

            BackgroundJob.Enqueue(() => RegisterToRedis(user));

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            BackgroundJob.Enqueue(() => RemoveFromRedis(Context.ConnectionId));

            return base.OnDisconnectedAsync(exception);
        }

        public async Task<string> AnnounceAsync(string message)
        {
            return await Clients.All.ReceiveMessageAsync(message);
        }

        public async Task<string> EchoAsync(string message)
        {
            return await Clients.Caller.ReceiveMessageAsync(message);
        }

        private async Task RegisterToRedis(HubUser user)
        {
            await _connectionMultiplexer.GetDatabase().StringSetAsync($"MessagingHub:Users:{user.ConnectionId}", JsonConvert.SerializeObject(user));
        }
        
        private async Task RemoveFromRedis(string connectionId)
        {
            await _connectionMultiplexer.GetDatabase().KeyDeleteAsync($"MessagingHub:Users:{Context.ConnectionId}");
        }
    }

    public class HubUser
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
    }
    public interface IMessagingHub
    {
        public Task<string> ReceiveMessageAsync(string message);
    }
}