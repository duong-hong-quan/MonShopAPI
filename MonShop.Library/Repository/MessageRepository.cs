using MonShop.Library.DAO;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.Repository
{
    public class MessageRepository: IMessageRepository
    {
        MessageDBContext db = new MessageDBContext();

        public async Task AddMessage(MessageRequest message) => await db.AddMessage(message);   
       
        
        public async Task<List<Message>> GetAllMessageByAccountID(int AccountID) => await db.GetAllMessageByAccountID(AccountID);


        public async Task<List<Message>> GetAllMessageByRoomID(int RoomID) => await db.GetAllMessageByRoomID(RoomID);
        public async Task<List<Room>> GetAllRoom() => await db.GetAllRoom();
        public async Task AddMessageAdmin(MessageAdminRequest message) => await db.AddMessageAdmin(message);
        public async Task<Room> GetRoomByID(int roomID) => await db.GetRoomByID(roomID);



    }
}
