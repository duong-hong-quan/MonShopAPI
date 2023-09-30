using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.Controller.Model;
using MonShop.Library.Models;
using MonShop.Library.Repository.IRepository;
using MonShopLibrary.DTO;

namespace MonShop.Controller.Controller
{
    [Route("Address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ResponseDTO _response;

        public AddressController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _response = new ResponseDTO();
        }

        /*
         * 
         *  public  Task AddAddress(DeliveryAddress addressDto);
        public Task UpdateAddress(DeliveryAddress addressDto);

        public Task RemoveAddress(DeliveryAddress addressDto);
        public  Task<List<DeliveryAddress>> GetAllAddressByUserId(string userId);*/

        [HttpPost("AddAddress")]
        public async Task<ResponseDTO> AddAddress(DeliveryAddress addressDto)
        {
            try
            {
                await _accountRepository.AddAddress(addressDto);
                _response.Data = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpPut("UpdateAddress")]
        public async Task<ResponseDTO> UpdateAddress(DeliveryAddress addressDto)
        {
            try
            {
                await _accountRepository.UpdateAddress(addressDto);
                _response.Data = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }


        [HttpPost("RemoveAddress")]
        public async Task<ResponseDTO> RemoveAddress(DeliveryAddress addressDto)
        {
            try
            {
                await _accountRepository.RemoveAddress(addressDto);
                _response.Data = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }
        [HttpGet("GetAllAddressByUserId/{UserId}")]
        public async Task<ResponseDTO> GetAllAddressByUserId(string UserId)
        {
            try
            {
                _response.Data = await _accountRepository.GetAllAddressByUserId(UserId);


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
