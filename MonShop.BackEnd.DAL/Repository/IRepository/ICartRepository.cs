using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Repository.IRepository
{
    public interface ICartRepository
    {
        public Task AddToCart(CartRequest request);
        public Task RemoveFromCart(CartRequest request);

        public Task RemoveCart(int CartId);

        public Task<IEnumerable<CartItem>> GetItemsByAccountId(string accountId);
        public Task<IEnumerable<CartItem>> GetItemsByCartId(int CartId);
        public Task UpdateCartItemById(CartRequest request);


    }
}
