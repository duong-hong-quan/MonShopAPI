using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.Controller.Model;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShop.Library.Repository.IRepository;
using System.Collections.Generic;

namespace MonShop.Controller.Controller
{
    [Route("Message")]
    [ApiController]

    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ResponseDTO _response;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
            _response = new ResponseDTO();
        }

        [HttpPost]
        [Route("AddMessage")]
        public async Task<ResponseDTO> AddMessage(MessageAdminRequest request)
        {
            try
            {
                await _messageRepository.AddMessageAdmin(request);
                _response.Data = request;


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }
        [HttpGet]
        [Route("GetMessageByRoomID/{roomID}")]

        public async Task<ResponseDTO> GetMessageByRoomID(int roomID)
        {
            try { }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            List<Message> list = await _messageRepository.GetAllMessageByRoomID(roomID);
            return _response;
        }

        [HttpGet]
        [Route("GetAllMessageByAccountID/{AccountID}")]

        public async Task<ResponseDTO> GetAllMessageByAccountID(string AccountID)
        {
            try
            {

                List<Message> list = await _messageRepository.GetAllMessageByAccountID(AccountID);
                _response.Data = list;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("GetAllRoom")]
        public async Task<ResponseDTO> GetAllRoom()
        {
            try
            {
                List<Room> list = await _messageRepository.GetAllRoom();
                _response.Data = list;


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetRoomByID/{roomID}")]
        public async Task<ResponseDTO> GetRoomByID(int roomID)
        {
            try { 
            Room room = await _messageRepository.GetRoomByID(roomID);
                _response.Data = room;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
