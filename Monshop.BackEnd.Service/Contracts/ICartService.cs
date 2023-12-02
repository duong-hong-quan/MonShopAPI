using MonShop.BackEnd.Common.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface ICartService
    {
        public Task<AppActionResult> UpdateCartItem(string accountId,IEnumerable<CartItemDto> cartItemDto);
        public Task<AppActionResult> GeCartItems(string accountId);
    }
}
