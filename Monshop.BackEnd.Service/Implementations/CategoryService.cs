using AutoMapper;
using Monshop.BackEnd.Service.Contracts;
using Monshop.BackEnd.Service.Services.Firebase;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.Common.Dto.Response;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.Utility.Utils;
using System.Transactions;

namespace Monshop.BackEnd.Service.Implementations
{
    public class CategoryService : GenericBackendService, ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IUnitOfWork _unitOfWork;
        private AppActionResult _result;
        private IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _result = new();
        }

        public async Task<AppActionResult> AddCategory(CategoryDto categoryDto)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var fileService = Resolve<IFirebaseService>();
                    bool isVaLid = true;
                    var categoryDb = await _categoryRepository.GetByExpression(c => c.CategoryName == categoryDto.CategoryName);
                    if (categoryDb != null)
                    {
                        isVaLid = false;
                        _result.Messages.Add("The category is existed");
                    }
                    else if (categoryDb == null && isVaLid)
                    {
                        var categoryMapper = _mapper.Map<Category>(categoryDto);
                        categoryMapper.IsDeleted = false;
                        await _categoryRepository.Insert(categoryMapper);
                        await _unitOfWork.SaveChangeAsync();
                        var pathName = SD.FirebasePathName.CATEGORY_PREFIX + categoryMapper.CategoryId;
                        var result = await fileService.UploadImageToFirebase(categoryDto.CategoryImgUrl, pathName);
                        if (result.IsSuccess && result.Result.Data != null)
                        {
                            _result.Messages.Add("Upload image to firebase successful");
                        }
                        categoryMapper.CategoryImgUrl = pathName;
                        await _categoryRepository.Update(categoryDb);
                        await _unitOfWork.SaveChangeAsync();
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

        public async Task<AppActionResult> DeleteCategory(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    bool isValid = true;
                    var categoryDb = await _categoryRepository.GetById(id);
                    if (categoryDb == null)
                    {
                        isValid = false;
                        _result.Messages.Add("The category is not existed");
                    }
                    else if (categoryDb != null && isValid)
                    {
                        categoryDb.IsDeleted = true;
                        await _categoryRepository.Update(categoryDb);
                        await _unitOfWork.SaveChangeAsync();
                    }
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

        public async Task<AppActionResult> GetAllCategory()
        {
            try
            {
                _result.Result.Data = await _categoryRepository.GetAll();   

            }
            catch (Exception ex) {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateCategory(CategoryDto categoryDto)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    bool isValid = true;
                    var fileService = Resolve<IFirebaseService>();
                    var categoryDb = await _categoryRepository.GetById(categoryDto.CategoryId);
                    if (categoryDb == null)
                    {
                        isValid = false;
                        _result.Messages.Add("The category is not existed");
                    }
                    else if (categoryDb != null && isValid)
                    {
                        var pathName = categoryDb.CategoryImgUrl;
                        var categoryMapper = _mapper.Map<Category>(categoryDto);
                        categoryDb = categoryMapper;
                        categoryDb.IsDeleted = false;
                        await _categoryRepository.Update(categoryDb);

                        var resultDelete = await fileService.DeleteImageFromFirebase(pathName);
                        if (resultDelete.IsSuccess && resultDelete.Result.Data != null)
                        {
                            _result.Messages.Add("Delete image to firebase successful");
                        }
                        var result = await fileService.UploadImageToFirebase(categoryDto.CategoryImgUrl, pathName);
                        if (result.IsSuccess && result.Result.Data != null)
                        {
                            _result.Messages.Add("Upload image to firebase successful");
                        }
                        await _unitOfWork.SaveChangeAsync();
                    }
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
