using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShopLibrary.DAO;
using MonShopLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility = MonShopLibrary.Utils.Utility;

namespace MonShop.Library.DAO
{
    public class MessageDBContext : MonShopContext
    {
        public MessageDBContext() { }


        public async Task AddMessage(MessageRequest message)
        {
            Message mess = await this.Messages.Where(m => m.Sender == message.AccountID).FirstOrDefaultAsync();
            if (mess != null)
            {
                Message DTO = new Message
                {
                    RoomId = mess.RoomId,
                    Sender = message.AccountID,
                    Content = message.Message,
                    SendTime = Utility.getInstance().GetCurrentDateTimeInTimeZone(),

                };
                await this.Messages.AddAsync(DTO);
                await this.SaveChangesAsync();


            }
            else
            {
                Room room = new Room { RoomName = Guid.NewGuid().ToString() };
                await this.Rooms.AddAsync(room);
                await this.SaveChangesAsync();
                int roomID = room.RoomId;
                Message DTO = new Message
                {
                    RoomId = roomID,
                    Sender = message.AccountID,
                    Content = message.Message,
                    SendTime = Utility.getInstance().GetCurrentDateTimeInTimeZone(),

                };
                this.Messages.Add(DTO);
                await this.SaveChangesAsync();

            }
        }
        public async Task<List<Message>> GetAllMessageByAccountID(int AccountID)
        {
            List<Message> list = null;

            List<Message> messages = await this.Messages.Where(m => m.Sender == AccountID).ToListAsync();
            Message mess = messages.ElementAt(0);

            {
                list = await this.Messages.Where(m => m.RoomId == mess.RoomId).ToListAsync();

            }
            return list;
        }

        public async Task<List<Message>> GetAllMessageByRoomID(int RoomID)
        {
            List<Message> list = await this.Messages.Where(m => m.RoomId == RoomID).ToListAsync();
            return list;
        }
    }
}
