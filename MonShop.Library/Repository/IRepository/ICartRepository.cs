using MonShop.Library.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.Repository.IRepository
{
    public interface ICartRepository
    {
       public Task AddToCart(CartRequest request);
        public Task RemoveToCart(CartRequest request);

        public Task RemoveCart(int CartId);

        public Task<IEnumerable<CartItem>> GetItemsByCartId(int CartId);

    }
}
