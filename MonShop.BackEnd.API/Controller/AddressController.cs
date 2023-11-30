using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using Monshop.BackEnd.Service.Contracts;

namespace MonShop.BackEnd.API.Controller
{
    [Route("Address")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AddressController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /*
         * 
         *  public  Task AddAddress(DeliveryAddress addressDto);
        public Task UpdateAddress(DeliveryAddress addressDto);

        public Task RemoveAddress(DeliveryAddress addressDto);
        public  Task<List<DeliveryAddress>> GetAllAddressByUserId(string userId);*/

        [HttpPost("AddAddress")]
        public async Task<AppActionResult> AddAddress(DeliveryAddressDTO addressDto)
        {
            return await _accountService.AddAddress(addressDto);
        }

        [HttpPut("UpdateAddress")]
        public async Task<AppActionResult> UpdateAddress(DeliveryAddressDTO addressDto)
        {
            return await _accountService.UpdateAddress(addressDto);

        }


        [HttpDelete("RemoveAddress")]
        public async Task<AppActionResult> RemoveAddress(DeliveryAddressDTO addressDto)
        {
            return await _accountService.RemoveAddress(addressDto);

        }
        [HttpGet("GetAllAddressByUserId/{UserId}")]
        public async Task<AppActionResult> GetAllAddressByUserId(string UserId)
        {
            return await _accountService.GetAllAddressByUserId(UserId);
        }


    }
}
