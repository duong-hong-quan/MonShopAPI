using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.BackEnd.API.Model;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.IRepository;

namespace MonShop.BackEnd.API.Controller
{
    [Route("Address")]
    [ApiController]
    [Authorize]
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
        public async Task<ResponseDTO> AddAddress(DeliveryAddressDTO addressDto)
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
        public async Task<ResponseDTO> UpdateAddress(DeliveryAddressDTO addressDto)
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


        [HttpDelete("RemoveAddress")]
        public async Task<ResponseDTO> RemoveAddress(DeliveryAddressDTO addressDto)
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
