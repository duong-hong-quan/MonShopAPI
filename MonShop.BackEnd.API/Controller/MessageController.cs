using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;

namespace MonShop.BackEnd.API.Controller
{
    [Route("Message")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [Route("AddMessage")]
        public async Task<AppActionResult> AddMessage(MessageAdminRequest request)
        {
          return  await _messageService.AddMessageAdmin(request);


        }
        [HttpGet]
        [Route("GetMessageByRoomID/{roomID}")]

        public async Task<AppActionResult> GetMessageByRoomID(int roomID)
        {
            return await _messageService.GetAllMessageByRoomID(roomID);

        }

        [HttpGet]
        [Route("GetAllMessageByAccountID/{AccountID}")]

        public async Task<AppActionResult> GetAllMessageByAccountID(string AccountID)
        {
            return await _messageService.GetAllMessageByAccountID(AccountID);

        }

        [HttpGet]
        [Route("GetAllRoom")]
        public async Task<AppActionResult> GetAllRoom()
        {
            return await _messageService.GetAllRoom();

        }

        [HttpGet]
        [Route("GetRoomByID/{roomID}")]
        public async Task<AppActionResult> GetRoomByID(int roomID)
        {
            return await _messageService.GetRoomByID(roomID);

        }
    }
}
