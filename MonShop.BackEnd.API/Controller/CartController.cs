using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.Common.Dto.Request;

namespace MonShop.BackEnd.API.Controller
{
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("update-cart-items")]
        public async Task<AppActionResult> UpdateCartItems(string accountId, IEnumerable<CartItemDto> cartItemDtos)
        {
            return await _cartService.UpdateCartItem(accountId, cartItemDtos);
        }
        [HttpPost("get-cart-items")]
        public async Task<AppActionResult> GeCartItems(string accountId)
        {
            return await _cartService.GeCartItems(accountId);
        }

    }
}
