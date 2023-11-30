using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface IMessageService
    {
        public Task<AppActionResult> AddMessage(MessageRequest message);


        public Task<AppActionResult> GetAllMessageByAccountID(string AccountID);


        public Task<AppActionResult> GetAllMessageByRoomID(int RoomID);

        public Task<AppActionResult> GetAllRoom();
        public Task<AppActionResult> AddMessageAdmin(MessageAdminRequest message);
        public Task<AppActionResult> GetRoomByID(int roomID);
        public Task<AppActionResult> CreateRoom(RoomDTO room);
        public Task<AppActionResult> UpdateRoom(Room room);
        public Task<AppActionResult> DeleteRoom(int RoomID);
    }
}
