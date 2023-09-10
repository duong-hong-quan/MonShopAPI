using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.Controller.Model;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShop.Library.Repository;
using System.Collections.Generic;

namespace MonShop.Controller.Controller
{
    [Route("Message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ResponeDTO _responeDTO;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
            _responeDTO = new ResponeDTO();
        }

        [HttpPost]
        [Route("AddMessage")]
        public async Task<ResponeDTO> AddMessage(MessageAdminRequest request)
        {
            try
            {
                await _messageRepository.AddMessageAdmin(request);
                _responeDTO.Data = request;


            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;

        }
        [HttpGet]
        [Route("GetMessageByRoomID")]

        public async Task<ResponeDTO> GetMessageByRoomID(int roomID)
        {
            try { }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            List<Message> list = await _messageRepository.GetAllMessageByRoomID(roomID);
            return _responeDTO;
        }

        [HttpGet]
        [Route("GetAllMessageByAccountID")]

        public async Task<ResponeDTO> GetAllMessageByAccountID(int AccountID)
        {
            try
            {

                List<Message> list = await _messageRepository.GetAllMessageByAccountID(AccountID);
                _responeDTO.Data = list;
            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }

            return _responeDTO;
        }

        [HttpGet]
        [Route("GetAllRoom")]
        public async Task<ResponeDTO> GetAllRoom()
        {
            try
            {
                List<Room> list = await _messageRepository.GetAllRoom();
                _responeDTO.Data = list;


            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }

        [HttpGet]
        [Route("GetRoomByID")]
        public async Task<ResponeDTO> GetRoomByID(int roomID)
        {
            try { 
            Room room = await _messageRepository.GetRoomByID(roomID);
                _responeDTO.Data = room;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
    }
}
