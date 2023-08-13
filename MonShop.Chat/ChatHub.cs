using Microsoft.AspNetCore.SignalR;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShop.Library.Repository;

namespace MonShop.Chat
{

    public class ChatHub : Hub
    {
        private readonly IMessageRepository messageRepository;
        public ChatHub() {
            messageRepository = new MessageRepository();
        }
        public async Task SendMessage(MessageRequest message)
        {
            await messageRepository.AddMessage(message);
            List<Message> list = await messageRepository.GetAllMessageByRoomID(1);
            await Clients.All.SendAsync("ReceiveMessage", list);
        }
    }
}
