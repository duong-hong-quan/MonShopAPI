using Microsoft.AspNetCore.SignalR;
using MonShop.Library.Models;

namespace MonShop.Chat
{

    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
