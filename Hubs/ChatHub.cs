
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ChatAppMVC.Hubs
{
    public class ChatHub : Hub
    {
        // Store the mapping of UserIds to connection IDs
        private static readonly ConcurrentDictionary<int, string> UserConnections = new();

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var userId = httpContext.Session.GetInt32("UserId");
                if (userId.HasValue)
                {

                    UserConnections[userId.Value] = Context.ConnectionId;
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var userId = httpContext.Session.GetInt32("UserId");
                if (userId.HasValue)
                {
                    UserConnections.TryRemove(userId.Value, out _);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToUser(int recipientUserId, string message)
        {
            if (UserConnections.TryGetValue(recipientUserId, out var connectionId))
            {
                var httpContext = Context.GetHttpContext();
                var senderUserId = httpContext?.Session.GetInt32("UserId");
                if (senderUserId.HasValue)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderUserId.Value, message);
                }
            }
        }
        public async Task NotifyTyping(int recipientUserId)
        {
            if (UserConnections.TryGetValue(recipientUserId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("Typing", recipientUserId==1?"User 2":"User 1");
            }
        }
       
    }
}
