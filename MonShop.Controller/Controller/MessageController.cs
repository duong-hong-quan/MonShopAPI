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
        public async Task<IActionResult> AddMessage(MessageRequest request)
        {
            await _messageRepository.AddMessage(request);
            return Ok();

        }
        [HttpGet]
        [Route("GetMessage")]

        public async Task<IActionResult> GetMessage(int roomID)
        {
            List<Message> list = await _messageRepository.GetAllMessageByRoomID(roomID);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllMessageByAccountID")]

        public async Task <IActionResult> GetAllMessageByAccountID(int AccountID)
        {
            List<Message> list = await _messageRepository.GetAllMessageByAccountID( AccountID);
            return Ok(list);
        }
    }
}
