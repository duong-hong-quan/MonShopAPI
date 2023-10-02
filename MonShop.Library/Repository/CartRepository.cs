using Microsoft.EntityFrameworkCore;
using MonShop.Library.Data;
using MonShop.Library.DTO;
using MonShop.Library.Models;
using MonShop.Library.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly MonShopContext _db;

        public CartRepository(MonShopContext db)
        {
            _db = db;
        }

        public async Task AddToCart(CartRequest request)
        {
            Cart cart = await _db.Cart.FirstOrDefaultAsync(c => c.ApplicationUserId == request.ApplicationUserId);
            CartItem item = await _db.CartItem.FirstOrDefaultAsync(i => i.ProductId == request.item.ProductId && i.SizeId == request.item.SizeId);

            if (cart == null)
            {
                cart = new Cart { ApplicationUserId = request.ApplicationUserId };
                await _db.Cart.AddAsync(cart);
                await _db.SaveChangesAsync();

            }
            if (item == null)
            {
                item = new CartItem { CartId = cart.CartId, ProductId = request.item.ProductId, Quantity = request.item.Quantity, SizeId = request.item.SizeId };
                await _db.CartItem.AddAsync(item);
                await _db.SaveChangesAsync();
            }
            else
            {
                item.Quantity += request.item.Quantity;
                await _db.SaveChangesAsync();

            }

        }

        private async Task<bool> IsOutOfStock(int ProductId, int SizeId, int quantity)
        {
            var productInventory = await _db.ProductInventory.SingleOrDefaultAsync(p => p.ProductId == ProductId && p.SizeId == SizeId);
            if (productInventory != null)
            {
                if (quantity > productInventory.Quantity)
                {
                    return true;

                }
                else if (quantity <= productInventory.Quantity)
                {
                    return false;

                }
            }
            return true;

        }

        public async Task<IEnumerable<CartItem>> GetItemsByAccountId(string AccountId)
        {
            double total = 0;
            Cart cart = await _db.Cart.FirstOrDefaultAsync(c => c.ApplicationUserId == AccountId);
            var list = await _db.CartItem.Where(c => c.CartId == cart.CartId).Include(c => c.Product).Include(c => c.Size).ToListAsync();



            List<CartItem> itemsToRemove = new List<CartItem>();

            foreach (var item1 in list)
            {
                var dupplicateList = await _db.CartItem.Where(c => c.ProductId == item1.ProductId && c.SizeId == item1.SizeId && c.CartItemId != item1.CartItemId).Include(c => c.Product).Include(c => c.Size).ToListAsync();

                if (dupplicateList.Count() > 0)
                {
                    foreach (var item in dupplicateList)
                    {
                        var itemDelete = itemsToRemove.FirstOrDefault(c => c.ProductId == item.ProductId && c.SizeId == item.SizeId);
                        if (itemDelete == null)
                        {
                            itemsToRemove.Add(item);
                            item1.Quantity += item.Quantity;
                        }
                    }

                }

            }

            foreach (var itemToRemove in itemsToRemove)
            {
                list.Remove(itemToRemove);
                _db.CartItem.Remove(itemToRemove);
            }

            await _db.SaveChangesAsync();

            foreach (var item in list)
            {
                item.IsOutOfStock = await IsOutOfStock((int)item.ProductId, item.SizeId, item.Quantity);
                if ((bool)!item.IsOutOfStock)
                {
                    total += (double)(item.Quantity * item.Product.Price * (100 - item.Product.Discount) / 100);
                }
            }

            cart.Total = total;
            return list;
        }


        public async Task<IEnumerable<CartItem>> GetItemsByCartId(int CartId)
        {
            return await _db.CartItem.Where(c => c.CartId == CartId).ToListAsync();
        }

        public async Task RemoveCart(int CartId)
        {
            Cart cart = await _db.Cart.FirstOrDefaultAsync(c => c.CartId == CartId);
            if (cart != null)
            {
                IEnumerable<CartItem> items = await _db.CartItem.Where(i => i.CartId == cart.CartId).ToListAsync();
                if (items.Count() > 0)
                {
                    foreach (CartItem item in items)
                    {
                        _db.CartItem.Remove(item);
                    }
                }

                _db.Cart.Remove(cart);
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCart(CartRequest request)
        {
            Cart cart = await _db.Cart.FirstOrDefaultAsync(c => c.ApplicationUserId == request.ApplicationUserId);
            CartItem item = await _db.CartItem.FirstOrDefaultAsync(i => i.ProductId == request.item.ProductId);

            List<CartItem> items = await _db.CartItem.Where(i => i.CartId == cart.CartId).ToListAsync();
            if (items.Count() <= 0)
            {
                _db.Cart.Remove(cart);
                await _db.SaveChangesAsync();
            }
            if (cart != null)
            {
                if (item != null && item.Quantity > 0)
                {
                    item.Quantity -= request.item.Quantity;
                }
                if (item.Quantity <= 0)
                {
                    _db.CartItem.Remove(item);
                }

                await _db.SaveChangesAsync();

            }
        }


        public async Task UpdateCartItemById(CartRequest request)
        {
            CartItem item = await _db.CartItem.FindAsync(request.item.CartItemId);
            if (item != null)
            {
                item.ProductId = request.item.ProductId;
                item.Quantity = request.item.Quantity;
                item.SizeId = request.item.SizeId;
                item.CartId = request.item.CartId;
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }

        }
    }
}
