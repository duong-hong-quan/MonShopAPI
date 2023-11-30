using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.IRepository;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Implementations
{
    public class CartService : GenericBackEndService, ICartService
    {
        private ICartRepository _cartRepository;
        private IUnitOfWork _unitOfWork;
        private AppActionResult _result;
        public CartService(IServiceProvider serviceProvider, ICartRepository cartRepository, IUnitOfWork unitOfWork) : base(serviceProvider)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _unitOfWork = unitOfWork;
            _result = new();
        }

        public async Task<AppActionResult> AddToCart(CartRequest request)
        {
            try
            {
                var cart = await _cartRepository.GetByExpression(c => c.ApplicationUserId == request.ApplicationUserId);
                var cartItemRepository = Resolve<ICartItemRepository>();
                var item = await cartItemRepository.GetByExpression(i => i.ProductId == request.item.ProductId && i.SizeId == request.item.SizeId);

                if (cart == null)
                {
                    cart = new Cart { ApplicationUserId = request.ApplicationUserId };
                    await _cartRepository.Insert(cart);

                }
                if (item == null)
                {
                    item = new CartItem { CartId = cart.CartId, ProductId = request.item.ProductId, Quantity = request.item.Quantity, SizeId = request.item.SizeId };
                    await cartItemRepository.Insert(item);
                }
                else
                {
                    item.Quantity += request.item.Quantity;

                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            finally
            {
                await _unitOfWork.SaveChangeAsync();
            }

        }

        public async Task<AppActionResult> GetItemsByAccountId(string accountId)
        {
            try
            {
                var cartItemRepository = Resolve<ICartItemRepository>();
                double total = 0;
                var cart = await _cartRepository.GetByExpression(c => c.ApplicationUserId == accountId);
                var list = await cartItemRepository.GetListByExpression(c => c.CartId == cart.CartId, c => c.Product, c => c.Size);



                List<CartItem> itemsToRemove = new List<CartItem>();

                foreach (var item1 in list)
                {
                    var dupplicateList = await cartItemRepository.GetListByExpression(c => c.ProductId == item1.ProductId && c.SizeId == item1.SizeId && c.CartItemId != item1.CartItemId, c => c.Product, c => c.Size);

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
                    await cartItemRepository.DeleteById(itemToRemove.CartItemId);
                }

                await _unitOfWork.SaveChangeAsync();
                foreach (var item in list)
                {
                    item.IsOutOfStock = await _cartRepository.IsOutOfStock((int)item.ProductId, item.SizeId, item.Quantity);
                    if ((bool)!item.IsOutOfStock)
                    {
                        total += (double)(item.Quantity * item.Product.Price * (100 - item.Product.Discount) / 100);
                    }
                }

                cart.Total = total;
                _result.Data = list;
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public Task<AppActionResult> GetItemsByCartId(int CartId)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> RemoveCart(int CartId)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> RemoveFromCart(CartRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> UpdateCartItemById(CartRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
