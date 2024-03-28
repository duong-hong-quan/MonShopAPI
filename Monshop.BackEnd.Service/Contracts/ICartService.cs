using MonShop.BackEnd.Common.Dto.Request;

namespace Monshop.BackEnd.Service.Contracts;

public interface ICartService
{
    public Task<AppActionResult> UpdateCartItem(string accountId, IEnumerable<CartItemDto> cartItemDto);
    public Task<AppActionResult> GeCartItems(string accountId);
}