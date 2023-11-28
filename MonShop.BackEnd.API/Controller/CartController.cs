using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonShop.BackEnd.API.Model;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Repository.IRepository;

namespace MonShop.BackEnd.API.Controller
{
    [Authorize]
    [Route("Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ResponseDTO _response;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            _response = new ResponseDTO();
        }
        [HttpPost("AddToCart")]
        public async Task<ResponseDTO> AddToCart(CartRequest request)

        {
            try
            {

                await _cartRepository.AddToCart(request);
                _response.Data = true;
            }
            catch (Exception ex)

            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpPost("RemoveFromCart")]

        public async Task<ResponseDTO> RemoveFromCart(CartRequest request)
        {
            try
            {
                await _cartRepository.RemoveFromCart(request);

            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;

        }
        [HttpPost("RemoveCart")]

        public async Task<ResponseDTO> RemoveCart(int CartId)
        {

            try
            {
                await _cartRepository.RemoveCart(CartId);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;

        }
        [HttpGet("GetItemsByAccountId/{AccountId}")]

        public async Task<ResponseDTO> GetItemsByAccountId(string AccountId)
        {
            try
            {
                var list = await _cartRepository.GetItemsByAccountId(AccountId);
                _response.Data = list;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;

            }
            return _response;

        }

        [HttpPut("UpdateCartItem")]
        public async Task<ResponseDTO> UpdateCartItemById(CartRequest request)
        {
            try
            {
                await _cartRepository.UpdateCartItemById(request);
                _response.Data = true;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

    }
}
