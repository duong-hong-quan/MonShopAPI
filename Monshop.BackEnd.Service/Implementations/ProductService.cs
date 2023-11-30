using AutoMapper;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.IRepository;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonShop.BackEnd.Utility.Utils.Constant;
using Product = MonShop.BackEnd.DAL.Models.Product;

namespace Monshop.BackEnd.Service.Implementations
{
    public class ProductService : GenericBackEndService, IProductService
    {
        private IProductRepository _productRepository;
        private IUnitOfWork _unitOfWork;
        private AppActionResult _result;
        private IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _result = new AppActionResult();
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> AddProduct(ProductDTO dto)
        {
            await _productRepository.Insert(_mapper.Map<Product>(dto));
            await _unitOfWork.SaveChangeAsync();
            return _result;
        }

        public async Task<AppActionResult> DeleteProduct(ProductDTO dto)
        {
            await _productRepository.DeleteById(dto.ProductId);
            await _unitOfWork.SaveChangeAsync();
            return _result;
        }

        public async Task<AppActionResult> GetAllCategory()
        {
            var categoryRepository = Resolve<ICategoryRepository>();
            _result.Data = await categoryRepository.GetAll();
            return _result;
        }

        public async Task<AppActionResult> GetAllProduct()
        {
            _result.Data = await _productRepository.GetByExpression(p => p.IsDeleted == false);
            return _result;
        }

        public async Task<AppActionResult> GetAllProductByCategoryId(int CategoryId)
        {
            _result.Data = await _productRepository.GetByExpression(c => c.CategoryId == CategoryId);
            return _result;
        }

        public async Task<AppActionResult> GetAllProductByManager()
        {
            _result.Data = await _productRepository.GetAll();
            return _result;
        }

        public async Task<AppActionResult> GetAllProductStatus()
        {
            var productStatusRepository = Resolve<IProductStatusRepository>();
            _result.Data = await productStatusRepository.GetAll();
            return _result;
        }

        public async Task<AppActionResult> GetAllSize()
        {
            var sizeRepository = Resolve<ISizeRepository>();
            _result.Data = await sizeRepository.GetAll();
            return _result;
        }

        public async Task<AppActionResult> GetProductByID(int id)
        {
            _result.Data = await _productRepository.GetById(id);
            return _result;
        }

        public async Task<AppActionResult> GetProductInventory(int ProductId, int SizeId)
        {
            var productInventoryRepository = Resolve<IProductInventoryRepository>();
            _result.Data = await productInventoryRepository.GetByExpression(i => i.ProductId == ProductId && i.SizeId == SizeId, p => p.Size, p => p.Product.ProductStatus, p => p.Product.Category);
            return _result;
        }

        public async Task<AppActionResult> GetTopXProduct(int x)
        {
            var newProduct = await _productRepository.GetListByExpression(p => p.IsDeleted == false
                                    && p.ProductStatusId != MonShop.BackEnd.Utility.Utils.Constant.Product.IN_ACTIVE,
                                    p => p.ProductStatus, p => p.Category);
            newProduct.OrderByDescending(p => p.ProductId).Take(x);


            _result.Data = newProduct;

            return _result;
        }

        public async Task<AppActionResult> UpdateProduct(ProductDTO dto)
        {
            await _productRepository.Update(_mapper.Map<Product>(dto));
            await _unitOfWork.SaveChangeAsync();
            return _result;
        }
    }
}
