using System.Transactions;
using AutoMapper;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.DAL.Models;
using Monshop.BackEnd.Service.Contracts;
using NetCore.QK.BackEndCore.Application.IRepositories;
using NetCore.QK.BackEndCore.Application.IUnitOfWork;

namespace Monshop.BackEnd.Service.Implementations;

public class CartService : GenericBackendService, ICartService
{
    private readonly IRepository<CartItem> _cartItemRepository;
    private readonly IRepository<Cart> _cartRepository;
    private readonly IMapper _mapper;
    private readonly AppActionResult _result;
    private readonly IUnitOfWork _unitOfWork;

    public CartService
    (
        IRepository<Cart> cartRepository,
        IRepository<CartItem> cartItemRepository,
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
            _result.Data = new
            {
                Cart = await _cartRepository.GetByExpression(c => c.ApplicationUserId == accountId),
                Items = await _cartItemRepository.GetAllDataByExpression(c => c.Cart.ApplicationUserId == accountId, 1,
                    100, null)
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
                var productInventoryRepository = Resolve<IRepository<ProductInventory>>();
                var cart = await _cartRepository.GetByExpression(c => c.ApplicationUserId == accountId);
                if (cart != null)
                {
                    var itemsIncart =
                        await _cartItemRepository.GetAllDataByExpression(c => c.CartId == cart.CartId, 0, 0, null);
                    if (itemsIncart.Items.Count() > 0)
                    {
                        foreach (var item in itemsIncart.Items)
                        foreach (var cartItem in cartItemDto)
                            if (cartItem.ProductId == item.ProductId && cartItem.SizeId == item.SizeId)
                            {
                                var productInventory = await productInventoryRepository.GetByExpression(p =>
                                    p.ProductId == cartItem.ProductId && p.SizeId == cartItem.SizeId);
                                if (productInventory.Quantity < cartItem.Quantity)
                                {
                                    _result.Messages.Add(
                                        $"The product with id {cartItem.ProductId} and size id{cartItem.SizeId} is out of stock");
                                }
                                else
                                {
                                    if (item.Quantity - cartItem.Quantity > 0)
                                    {
                                        item.Quantity += cartItem.Quantity;
                                        if (item.Quantity <= 0)
                                        {
                                            await _cartItemRepository.DeleteById(item.CartItemId);
                                            await _unitOfWork.SaveChangesAsync();
                                        }
                                    }

                                    else if (item.Quantity - cartItem.Quantity <= 0)
                                    {
                                        await _cartItemRepository.DeleteById(item.CartItemId);
                                        await _unitOfWork.SaveChangesAsync();
                                    }
                                }
                            }

                        if (!itemsIncart.Items.Any())
                        {
                            await _cartRepository.DeleteById(cart.CartId);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        await _unitOfWork.SaveChangesAsync();
                    }
                }
                else
                {
                    var cartForUser = new Cart { ApplicationUserId = accountId };
                    await _cartRepository.Insert(cartForUser);
                    await _unitOfWork.SaveChangesAsync();
                    var cartItemInsert = _mapper.Map<IEnumerable<CartItem>>(cartItemDto);
                    foreach (var cartItem in cartItemInsert) cartItem.CartId = cartForUser.CartId;
                    await _cartItemRepository.InsertRange(cartItemInsert);
                    await _unitOfWork.SaveChangesAsync();
                }

                _result.Data =
                    await _cartItemRepository.GetAllDataByExpression(c => c.Cart.ApplicationUserId == accountId, 0, 0,
                        null);
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