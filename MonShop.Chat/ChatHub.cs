using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MonShop.Chat
{
    [HubName("MyHub")]

    public class ChatHub : Hub
    {
        public async Task SendMessage(int messageId, int? sender, int? receiver, DateTime? sendTime, string message)
        {
            // Store the message and other details in your data store
            // You can use a database or an in-memory list to store the messages

            // Broadcast the received message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", messageId, sender, receiver, sendTime, message);
        }
    }
}
