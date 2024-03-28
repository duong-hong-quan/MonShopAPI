using Microsoft.AspNetCore.Mvc;
using MonShop.BackEnd.Common.Dto.Request;
using Monshop.BackEnd.Service.Contracts;

namespace MonShop.BackEnd.API.Controller;

[Route("cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

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