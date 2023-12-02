using AutoMapper;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.Models;
using System.Transactions;

namespace Monshop.BackEnd.Service.Implementations
{
    public class CartService : GenericBackendService, ICartService
    {
        private ICartRepository _cartRepository;
        private ICartItemRepository _cartItemRepository;
        private IUnitOfWork _unitOfWork;
        private AppActionResult _result;
        private IMapper _mapper;

        public CartService
        (
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _unitOfWork = unitOfWork;
            _result = new AppActionResult();
            _mapper = mapper;
        }

        public async Task<AppActionResult> GeCartItems(string accountId)
        {
            try
            {
                _result.Result.Data = new
                {
                    Cart = await _cartRepository.GetByExpression(c => c.ApplicationUserId == accountId),
                    Items = await _cartItemRepository.GetListByExpression(c => c.Cart.ApplicationUserId == accountId)
                };
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateCartItem(string accountId, IEnumerable<CartItemDto> cartItemDto)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var productInventoryRepository = Resolve<IProductInventoryRepository>();
                    var cart = await _cartRepository.GetByExpression(c => c.ApplicationUserId == accountId);
                    if (cart != null)
                    {
                        var itemsIncart = await _cartItemRepository.GetListByExpression(c => c.CartId == cart.CartId);
                        if (itemsIncart.Count() > 0)
                        {
                            foreach (var item in itemsIncart)
                            {
                                foreach (var cartItem in cartItemDto)
                                {
                                    if (cartItem.ProductId == item.ProductId && cartItem.SizeId == item.SizeId)
                                    {
                                        var productInventory = await productInventoryRepository.GetByExpression(p => p.ProductId == cartItem.ProductId && p.SizeId == cartItem.SizeId);
                                        if (productInventory.Quantity < cartItem.Quantity)
                                        {
                                            _result.Messages.Add($"The product with id {cartItem.ProductId} and size id{cartItem.SizeId} is out of stock");
                                        }
                                        else
                                        {
                                            if (item.Quantity - cartItem.Quantity > 0)
                                            {
                                                item.Quantity += cartItem.Quantity;
                                                if (item.Quantity <= 0)
                                                {
                                                    await _cartItemRepository.DeleteById(item.CartItemId);
                                                    await _unitOfWork.SaveChangeAsync();
                                                }
                                            }

                                            else if (item.Quantity - cartItem.Quantity <= 0)
                                            {
                                                await _cartItemRepository.DeleteById(item.CartItemId);
                                                await _unitOfWork.SaveChangeAsync();

                                            }
                                        }
                                    }
                                }
                            }
                            if (!itemsIncart.Any())
                            {
                                await _cartRepository.DeleteById(cart.CartId);
                                await _unitOfWork.SaveChangeAsync();
                            }
                            await _unitOfWork.SaveChangeAsync();

                        }
                    }
                    else
                    {
                        Cart cartForUser = new Cart { ApplicationUserId = accountId };
                        await _cartRepository.Insert(cartForUser);
                        await _unitOfWork.SaveChangeAsync();
                        IEnumerable<CartItem> cartItemInsert = _mapper.Map<IEnumerable<CartItem>>(cartItemDto);
                        foreach (var cartItem in cartItemInsert)
                        {
                            cartItem.CartId = cartForUser.CartId;
                        }
                        await _cartItemRepository.InsertRange(cartItemInsert);
                        await _unitOfWork.SaveChangeAsync();
                    }
                    _result.Result.Data = await _cartItemRepository.GetListByExpression(c => c.Cart.ApplicationUserId == accountId);
                    scope.Complete();

                }
                catch (Exception ex)
                {
                    _result.IsSuccess = false;
                    _result.Messages.Add(ex.Message);

                }

            }

            return _result;
        }

    }
}
