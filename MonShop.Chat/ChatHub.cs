using Microsoft.AspNetCore.SignalR;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShop.Library.Repository;

namespace MonShop.Chat
{

    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        public ChatHub(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task SendMessage(MessageRequest message)
        {
            await _messageRepository.AddMessage(message);
            List<Message> list = await _messageRepository.GetAllMessageByAccountID(message.AccountID);
            await Clients.All.SendAsync("ReceiveMessage", list);
            await Clients.All.SendAsync("ReceiveAdminMessage", list);
            List<Room> roomList = await _messageRepository.GetAllRoom();
            await Clients.All.SendAsync("ReceiveAllRoom", roomList);
        }


        public async Task AddMessageAdmin(MessageAdminRequest message)
        {

            await _messageRepository.AddMessageAdmin(message);
            List<Message> list = await _messageRepository.GetAllMessageByRoomID(message.RoomId);
            await Clients.All.SendAsync("ReceiveMessage", list);
            await Clients.All.SendAsync("ReceiveAdminMessage", list);

        }
        public async Task UpdateRoom(Room room)
        {
            await _messageRepository.UpdateRoom(room);
            List<Room> roomList = await _messageRepository.GetAllRoom();

            await Clients.All.SendAsync("ReceiveAllRoom", roomList);

        }
        public async Task DeleteRoom(int RoomID)
        {

            await _messageRepository.DeleteRoom(RoomID);
            List<Room> roomList = await _messageRepository.GetAllRoom();

            await Clients.All.SendAsync("ReceiveAllRoom", roomList);
        }



        public async Task CreateRoom(RoomDTO room)
        {
            await _messageRepository.CreateRoom(room);
            List<Room> roomList = await _messageRepository.GetAllRoom();

            await Clients.All.SendAsync("ReceiveAllRoom", roomList);

        }
    }
}
