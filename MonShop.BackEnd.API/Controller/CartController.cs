using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;

namespace MonShop.BackEnd.API.Controller
{
    [Authorize]
    [Route("Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost("AddToCart")]
        public async Task<AppActionResult> AddToCart(CartRequest request)
        {
            return await _cartService.AddToCart(request);
        }
        [HttpPost("RemoveFromCart")]

        public async Task<AppActionResult> RemoveFromCart(CartRequest request)
        {
            return await _cartService.RemoveFromCart(request);


        }
        [HttpPost("RemoveCart")]

        public async Task<AppActionResult> RemoveCart(int CartId)
        {
            return await _cartService.RemoveCart(CartId);


        }
        [HttpGet("GetItemsByAccountId/{AccountId}")]

        public async Task<AppActionResult> GetItemsByAccountId(string AccountId)
        {
            return await _cartService.GetItemsByAccountId(AccountId);


        }

        [HttpPut("UpdateCartItem")]
        public async Task<AppActionResult> UpdateCartItemById(CartRequest request)
        {
            return await _cartService.UpdateCartItemById(request);

        }

    }
}
