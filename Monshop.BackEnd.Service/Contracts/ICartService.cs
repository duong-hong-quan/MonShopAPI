using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface ICartService
    {
        public Task<AppActionResult> AddToCart(CartRequest request);
        public Task<AppActionResult> RemoveFromCart(CartRequest request);

        public Task<AppActionResult> RemoveCart(int CartId);

        public Task<AppActionResult> GetItemsByAccountId(string accountId);
        public Task<AppActionResult> GetItemsByCartId(int CartId);
        public Task<AppActionResult> UpdateCartItemById(CartRequest request);
    }
}
