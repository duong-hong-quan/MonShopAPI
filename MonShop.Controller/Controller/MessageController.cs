using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShop.Library.Repository;

namespace MonShop.Controller.Controller
{
    [Route("Message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController()
        {
            _messageRepository = new MessageRepository();
        }

        [HttpPost]
        [Route("AddMessage")]
        public async Task<IActionResult> AddMessage(MessageAdminRequest request)
        {
            await _messageRepository.AddMessageAdmin(request);
            return Ok();

        }
        [HttpGet]
        [Route("GetMessageByRoomID")]

        public async Task<IActionResult> GetMessageByRoomID(int roomID)
        {
            List<Message> list = await _messageRepository.GetAllMessageByRoomID(roomID);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllMessageByAccountID")]

        public async Task<IActionResult> GetAllMessageByAccountID(int AccountID)
        {
            List<Message> list = await _messageRepository.GetAllMessageByAccountID(AccountID);
            if (list != null)
            {
                return Ok(list);

            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAllRoom")]
        public async Task<IActionResult> GetAllRoom()
        {
            List<Room> list = await _messageRepository.GetAllRoom();
            return Ok(list);
        }

        [HttpGet]
        [Route("GetRoomByID")]
        public async Task<IActionResult> GetRoomByID(int roomID)
        {
            Room room = await _messageRepository.GetRoomByID(roomID);
            return Ok(room);
        }
    }
}
