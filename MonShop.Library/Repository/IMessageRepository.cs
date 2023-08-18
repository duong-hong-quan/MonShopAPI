using MonShop.Library.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.Repository
{
    public interface IMessageRepository
    {
        public Task AddMessage(MessageRequest message);


        public Task<List<Message>> GetAllMessageByAccountID(int AccountID);


        public Task<List<Message>> GetAllMessageByRoomID(int RoomID);

        public Task<List<Room>> GetAllRoom();
        public Task AddMessageAdmin(MessageAdminRequest message);
        public  Task<Room> GetRoomByID(int roomID);

    }
}
