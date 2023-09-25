using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonShop.Controller.Model;
using MonShop.Library.DTO;
using MonShop.Library.Repository.IRepository;

namespace MonShop.Controller.Controller
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
        [HttpPost("RemoveToCart")]

        public async Task<ResponseDTO> RemoveToCart(CartRequest request)
        {
            try
            {
                await _cartRepository.RemoveToCart(request);

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
    }
}
