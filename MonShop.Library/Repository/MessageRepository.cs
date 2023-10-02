using Microsoft.EntityFrameworkCore;
using MonShop.Library.Data;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShop.Library.Repository.IRepository;
using MonShopLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MonShopContext _db;

        public MessageRepository(MonShopContext db)
        {
            _db = db;
        }

        public async Task AddMessage(MessageRequest message)
        {
            Message mess = await _db.Message.Where(m => m.ApplicationUserId == message.AccountID).FirstOrDefaultAsync();
            if (mess != null)
            {
                Message DTO = new Message
                {
                    RoomId = mess.RoomId,
                    ApplicationUserId = message.AccountID,
                    Content = message.Message,
                    SendTime = Utility.getInstance().GetCurrentDateTimeInTimeZone(),

                };
                await _db.Message.AddAsync(DTO);
                await _db.SaveChangesAsync();


            }
            else
            {
                ApplicationUser account = await _db.Users.Where(a => a.Id == message.AccountID).FirstAsync();
                Room room = new Room { RoomName = $"{account.FirstName} {account.LastName}", RoomImg = "" };
                await _db.Room.AddAsync(room);
                await _db.SaveChangesAsync();
                int roomID = room.RoomId;
                Message DTO = new Message
                {
                    RoomId = roomID,
                    ApplicationUserId = message.AccountID,
                    Content = message.Message,
                    SendTime = Utility.getInstance().GetCurrentDateTimeInTimeZone(),

                };
                _db.Message.Add(DTO);
                await _db.SaveChangesAsync();

            }
        }

        public async Task<List<Room>> GetAllRoom()
        {
            List<Room> Room = await _db.Room.ToListAsync();
            return Room;
        }

        public async Task<Room> GetRoomByID(int roomID)
        {
            Room Room = await _db.Room.FirstOrDefaultAsync(r => r.RoomId == roomID);
            return Room;
        }
        public async Task AddMessageAdmin(MessageAdminRequest message)
        {
            Message mess = new Message
            {
                Content = message.Content,
                RoomId = message.RoomId,
                ApplicationUserId = message.Sender,
                SendTime = Utility.getInstance().GetCurrentDateTimeInTimeZone(),
            };
            await _db.Message.AddAsync(mess);
            await _db.SaveChangesAsync();

        }
        public async Task<List<Message>> GetAllMessageByAccountID(string AccountID)
        {
            List<Message> list = null;

            List<Message> Message = await _db.Message.Where(m => m.ApplicationUserId == AccountID).ToListAsync();
            if (Message.Count > 0)
            {
                Message mess = Message.ElementAt(0);
                if (mess != null)
                {
                    list = await _db.Message.Where(m => m.RoomId == mess.RoomId).ToListAsync();

                }
            }

            return list;
        }

        public async Task<List<Message>> GetAllMessageByRoomID(int RoomID)
        {
            List<Message> list = await _db.Message.Where(m => m.RoomId == RoomID).ToListAsync();
            return list;
        }
        public async Task CreateRoom(RoomDTO room)
        {
            Room DTO = new Room { RoomName = room.RoomName, RoomImg = room.RoomImg };
            await _db.Room.AddAsync(DTO);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateRoom(Room room)
        {
            _db.Room.Update(room);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteRoom(int RoomID)
        {
            List<Message> list = await GetAllMessageByRoomID(RoomID);
            foreach (Message message in list)
            {
                _db.Message.Remove(message);

            }
            Room room = await GetRoomByID(RoomID);
            _db.Room.Remove(room);
            await _db.SaveChangesAsync();
        }


    }
}
