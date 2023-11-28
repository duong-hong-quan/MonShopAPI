using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Repository.IRepository
{
    public interface IMessageRepository
    {
        public Task AddMessage(MessageRequest message);


        public Task<List<Message>> GetAllMessageByAccountID(string AccountID);


        public Task<List<Message>> GetAllMessageByRoomID(int RoomID);

        public Task<List<Room>> GetAllRoom();
        public Task AddMessageAdmin(MessageAdminRequest message);
        public Task<Room> GetRoomByID(int roomID);
        public Task CreateRoom(RoomDTO room);
        public Task UpdateRoom(Room room);
        public Task DeleteRoom(int RoomID);
    }
}
