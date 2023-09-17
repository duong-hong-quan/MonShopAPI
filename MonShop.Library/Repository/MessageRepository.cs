using Microsoft.EntityFrameworkCore;
using MonShop.Library.DTO;
using MonShop.Library.Models;
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
            Message mess = await _db.Messages.Where(m => m.Sender == message.AccountID).FirstAsync();
            if (mess != null)
            {
                Message DTO = new Message
                {
                    RoomId = mess.RoomId,
                    Sender = message.AccountID,
                    Content = message.Message,
                    SendTime = Utility.getInstance().GetCurrentDateTimeInTimeZone(),

                };
                await _db.Messages.AddAsync(DTO);
                await _db.SaveChangesAsync();


            }
            else
            {
                Account account = await _db.Accounts.Where(a => a.AccountId == message.AccountID).FirstAsync();
                Room room = new Room { RoomName = $"{account.FirstName} {account.LastName}", RoomImg = account.ImageUrl };
                await _db.Rooms.AddAsync(room);
                await _db.SaveChangesAsync();
                int roomID = room.RoomId;
                Message DTO = new Message
                {
                    RoomId = roomID,
                    Sender = message.AccountID,
                    Content = message.Message,
                    SendTime = Utility.getInstance().GetCurrentDateTimeInTimeZone(),

                };
                _db.Messages.Add(DTO);
                await _db.SaveChangesAsync();

            }
        }

        public async Task<List<Room>> GetAllRoom()
        {
            List<Room> rooms = await _db.Rooms.ToListAsync();
            return rooms;
        }

        public async Task<Room> GetRoomByID(int roomID)
        {
            Room rooms = await _db.Rooms.FirstAsync(r => r.RoomId == roomID);
            return rooms;
        }
        public async Task AddMessageAdmin(MessageAdminRequest message)
        {
            Message mess = new Message
            {
                Content = message.Content,
                RoomId = message.RoomId,
                Sender = message.Sender,
                SendTime = Utility.getInstance().GetCurrentDateTimeInTimeZone(),
            };
            await _db.Messages.AddAsync(mess);
            await _db.SaveChangesAsync();

        }
        public async Task<List<Message>> GetAllMessageByAccountID(int AccountID)
        {
            List<Message> list = null;

            List<Message> messages = await _db.Messages.Where(m => m.Sender == AccountID).ToListAsync();
            if (messages.Count > 0)
            {
                Message mess = messages.ElementAt(0);
                if (mess != null)
                {
                    list = await _db.Messages.Where(m => m.RoomId == mess.RoomId).ToListAsync();

                }
            }

            return list;
        }

        public async Task<List<Message>> GetAllMessageByRoomID(int RoomID)
        {
            List<Message> list = await _db.Messages.Where(m => m.RoomId == RoomID).ToListAsync();
            return list;
        }
        public async Task CreateRoom(RoomDTO room)
        {
            Room DTO = new Room { RoomName = room.RoomName, RoomImg = room.RoomImg };
            await _db.Rooms.AddAsync(DTO);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateRoom(Room room)
        {
            _db.Rooms.Update(room);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteRoom(int RoomID)
        {
            List<Message> list = await GetAllMessageByRoomID(RoomID);
            foreach (Message message in list)
            {
                _db.Messages.Remove(message);

            }
            Room room = await GetRoomByID(RoomID);
            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
        }


    }
}
