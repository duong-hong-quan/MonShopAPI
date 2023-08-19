using Microsoft.AspNetCore.SignalR;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShop.Library.Repository;

namespace MonShop.Chat
{

    public class ChatHub : Hub
    {
        private readonly IMessageRepository messageRepository;
        public ChatHub()
        {
            messageRepository = new MessageRepository();
        }
        public async Task SendMessage(MessageRequest message)
        {
            await messageRepository.AddMessage(message);
            List<Message> list = await messageRepository.GetAllMessageByAccountID(message.AccountID);
            await Clients.All.SendAsync("ReceiveMessage", list);
            await Clients.All.SendAsync("ReceiveAdminMessage", list);
            List<Room> roomList = await messageRepository.GetAllRoom();
            await Clients.All.SendAsync("ReceiveAllRoom", roomList);
        }


        public async Task AddMessageAdmin(MessageAdminRequest message)
        {

            await messageRepository.AddMessageAdmin(message);
            List<Message> list = await messageRepository.GetAllMessageByRoomID(message.RoomId);
            await Clients.All.SendAsync("ReceiveMessage", list);
            await Clients.All.SendAsync("ReceiveAdminMessage", list);

        }
        public async Task UpdateRoom(Room room)
        {
            await messageRepository.UpdateRoom(room);
            List<Room> roomList = await messageRepository.GetAllRoom();

            await Clients.All.SendAsync("ReceiveAllRoom", roomList);

        }
        public async Task DeleteRoom(int RoomID)
        {

            await messageRepository.DeleteRoom(RoomID);
            List<Room> roomList = await messageRepository.GetAllRoom();

            await Clients.All.SendAsync("ReceiveAllRoom", roomList);
        }



        public async Task CreateRoom(RoomDTO room)
        {
            await messageRepository.CreateRoom(room);
            List<Room> roomList = await messageRepository.GetAllRoom();

            await Clients.All.SendAsync("ReceiveAllRoom", roomList);

        }
    }
}
