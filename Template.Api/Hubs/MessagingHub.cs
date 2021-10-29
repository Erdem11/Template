using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using Template.Common.SettingsConfigurationFiles;
using Template.Data;

namespace Template.Api.Hubs
{
    public interface IMessagingHub
    {
        public Task ReceiveMessageAsync(string message);
    }

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

            BackgroundJob.Enqueue<MessagingHubHelper>((x) => x.AddToRedis(user));

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            BackgroundJob.Enqueue<MessagingHubHelper>((x) => x.RemoveFromRedis(Context.ConnectionId));

            return base.OnDisconnectedAsync(exception);
        }

        public async Task<string> AnnounceAsync(string message)
        {
            await Clients.Caller.ReceiveMessageAsync(message);
            return message;
        }

        public async Task<string> EchoAsync(string message)
        {
            BackgroundJob.Enqueue<MessagingHubHelper>(x => x.AddMessageToDb(new Guid(), message));
            await Clients.Caller.ReceiveMessageAsync(message);
            return message;
        }
    }

    public class HubUser
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
    }

}