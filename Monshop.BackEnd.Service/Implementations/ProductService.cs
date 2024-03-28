using System.Transactions;
using AutoMapper;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.DAL.Models;
using Monshop.BackEnd.Service.Contracts;
using Monshop.BackEnd.Service.Services.Firebase;
using MonShop.BackEnd.Utility.Utils;
using NetCore.QK.BackEndCore.Application.IRepositories;
using NetCore.QK.BackEndCore.Application.IUnitOfWork;

namespace Monshop.BackEnd.Service.Implementations;

public class ProductService : GenericBackendService, IProductService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Product> _productRepository;
    private readonly AppActionResult _result;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IRepository<Product> productRepository, IMapper mapper, IServiceProvider serviceProvider,
        IUnitOfWork unitOfWork) : base(serviceProvider)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _result = new AppActionResult();
        _unitOfWork = unitOfWork;
    }


    public async Task<AppActionResult> AddProduct(ProductDto product)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var fileService = Resolve<IFirebaseService>();
                var productInventoryRepository = Resolve<IRepository<ProductInventory>>();
                var isValid = true;
                if (await _productRepository.GetByExpression(p =>
                        p.ProductName.ToLower() == product.ProductName.ToLower()) != null)
                {
                    isValid = false;
                    _result.Messages.Add("The product with name is existed");
                }

                if (isValid)
                {
                    var productMapper = _mapper.Map<Product>(product);
                    await _productRepository.Insert(productMapper);
                    await _unitOfWork.SaveChangesAsync();

                    var productInventories = new List<ProductInventory>();
                    foreach (var item in product.Inventory)
                        productInventories.Add(
                            new ProductInventory
                            {
                                ProductId = productMapper.ProductId,
                                Quantity = item.Quantity,
                                SizeId = item.SizeId
                            });
                    await productInventoryRepository.InsertRange(productInventories);
                    await _unitOfWork.SaveChangesAsync();
                    var pathName = SD.FirebasePathName.PRODUCT_PREFIX + $"{productMapper.ProductId}.jpg";
                    productMapper.ImageUrl = pathName;
                    var upload = await fileService.UploadImageToFirebase(product.ImageUrl, pathName);
                    if (upload.IsSuccess && upload.Data != null)
                        _result.Messages.Add("Upload firebase successful");
                    _result.Messages.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);
                }

                scope.Complete();
            }
            catch (Exception ex)
            {
                _result.Messages.Add($"{ex.Message}");
                _result.IsSuccess = false;
            }
        }

        return _result;
    }

    public async Task<AppActionResult> DeleteProduct(int productId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var isValid = true;
                var productDb = await _productRepository.GetById(productId);
                if (productDb == null)
                {
                    isValid = false;
                    _result.Messages.Add(SD.ResponseMessage.NOTFOUND(productId, "product"));
                }
                else if (isValid)
                {
                    productDb.IsDeleted = true;
                    await _productRepository.Update(productDb);
                    await _unitOfWork.SaveChangesAsync();
                }

                scope.Complete();
            }
            catch (Exception ex)
            {
            }
        }

        return _result;
    }

    public async Task<AppActionResult> GetAllProductInventory()
    {
        try
        {
            var productInventoryRepository = Resolve<IRepository<ProductInventory>>();
            _result.Data =
                await productInventoryRepository.GetAllDataByExpression(null, 0, 0, p => p.Product, p => p.Size);
        }
        catch (Exception ex)
        {
            _result.IsSuccess = false;
            _result.Messages.Add(ex.Message);
        }

        return _result;
    }

    public async Task<AppActionResult> GetAllProductStatus()
    {
        try
        {
            var productStatusRepository = Resolve<IRepository<ProductStatus>>();
            _result.Data = await productStatusRepository.GetAllDataByExpression(null, 0, 0, null);
        }
        catch (Exception ex)
        {
            _result.IsSuccess = false;
            _result.Messages.Add(ex.Message);
        }

        return _result;
    }

    public async Task<AppActionResult> GetProductById(int id)
    {
        try
        {
            _result.Data = await _productRepository.GetById(id);
        }
        catch (Exception ex)
        {
            _result.IsSuccess = false;
            _result.Messages.Add(ex.Message);
        }

        return _result;
    }

    public async Task<AppActionResult> GetProductByManager()
    {
        try
        {
            _result.Data = await _productRepository.GetAllDataByExpression(null, 0, 0, null);
        }
        catch (Exception ex)
        {
            _result.IsSuccess = false;
            _result.Messages.Add(ex.Message);
        }

        return _result;
    }

    public async Task<AppActionResult> GetTopXProduct(int x)
    {
        try
        {
            var source = await _productRepository.GetAllDataByExpression(null, 0, 0, null);
            ;
            source.Items.OrderByDescending(o => o.ProductId).Take(x);
            _result.Data = source;
        }
        catch (Exception ex)
        {
            _result.IsSuccess = false;
            _result.Messages.Add(ex.Message);
        }

        return _result;
    }

    public async Task<AppActionResult> UpdateProduct(ProductDto product)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var fileService = Resolve<IFirebaseService>();
                var isValid = true;
                var productDb = await _productRepository.GetById(product.ProductId);
                if (productDb == null)
                {
                    isValid = false;
                    _result.Messages.Add(SD.ResponseMessage.NOTFOUND(product.ProductId, "product"));
                }
                else if (isValid)
                {
                    var result = await fileService.DeleteImageFromFirebase(productDb?.ImageUrl);
                    if (result.IsSuccess && result.Data != null)
                        _result.Messages.Add("Delete image on firebase cloud successful");
                    var upload = await fileService.UploadImageToFirebase(product.ImageUrl, productDb?.ImageUrl);
                    if (upload.IsSuccess && upload.Data != null)
                        _result.Messages.Add("Upload image on firebase cloud successful");
                    productDb = _mapper.Map<Product>(product);
                    await _productRepository.Update(productDb);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
        }

        return _result;
    }

    public async Task<AppActionResult> UpdateProductInventory(ProductInventoryDto inventoryDto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var isValid = true;
                var productInventoryRepository = Resolve<IRepository<ProductInventory>>();
                var productInventory = await productInventoryRepository
                    .GetByExpression(p => p.ProductId == inventoryDto.ProductId && p.SizeId == inventoryDto.SizeId);
                if (productInventory == null)
                {
                    isValid = false;
                    _result.Messages.Add(
                        SD.ResponseMessage.NOTFOUND_BY_FIELDNAME("Product Id & Size Id", "Product Inventory"));
                }
                else if (isValid)
                {
                    productInventory.Quantity = inventoryDto.Quantity;
                    await productInventoryRepository.Update(productInventory);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                }
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