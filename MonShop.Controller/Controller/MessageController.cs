using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShop.Library.Repository;

namespace MonShop.Controller.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController()
        {
            _messageRepository = new MessageRepository();
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(MessageRequest request)
        {
            await _messageRepository.AddMessage(request);
            return Ok();

        }
        [HttpGet]
        public async Task<IActionResult> GetMessage(int roomID)
        {
            List<Message> list = await _messageRepository.GetAllMessageByRoomID(roomID);
            return Ok(list);
        }
    }
}
