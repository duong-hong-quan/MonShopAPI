﻿using Microsoft.EntityFrameworkCore;
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

            if (cart == null )
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

        public async Task<IEnumerable<CartItem>> GetItemsByAccountId(string AccountId)
        {
            Cart cart = await _db.Cart.FirstAsync(c => c.ApplicationUserId == AccountId);
            return await _db.CartItem.Where(c => c.CartId == cart.CartId).ToListAsync();
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
            if(items.Count() <= 0)
            {
                _db.Cart.Remove(cart);
                await _db.SaveChangesAsync();
            }
            if (cart != null )
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
    }
}
