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

public class CategoryService : GenericBackendService, ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;
    private readonly AppActionResult _result;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IRepository<Category> categoryRepository, IUnitOfWork unitOfWork, IMapper mapper,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _result = new AppActionResult();
    }

    public async Task<AppActionResult> AddCategory(CategoryDto categoryDto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var fileService = Resolve<IFirebaseService>();
                var isVaLid = true;
                var categoryDb =
                    await _categoryRepository.GetByExpression(c => c.CategoryName == categoryDto.CategoryName);
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
                    await _unitOfWork.SaveChangesAsync();
                    var pathName = SD.FirebasePathName.CATEGORY_PREFIX + categoryMapper.CategoryId;
                    var result = await fileService.UploadImageToFirebase(categoryDto.CategoryImgUrl, pathName);
                    if (result.IsSuccess && result.Data != null)
                        _result.Messages.Add("Upload image to firebase successful");
                    categoryMapper.CategoryImgUrl = pathName;
                    await _categoryRepository.Update(categoryDb);
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

    public async Task<AppActionResult> DeleteCategory(int id)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var isValid = true;
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
                    await _unitOfWork.SaveChangesAsync();
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
            _result.Data = await _categoryRepository.GetAllDataByExpression(null, 0, 0, null);
        }
        catch (Exception ex)
        {
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
                var isValid = true;
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
                    if (resultDelete.IsSuccess && resultDelete.Data != null)
                        _result.Messages.Add("Delete image to firebase successful");
                    var result = await fileService.UploadImageToFirebase(categoryDto.CategoryImgUrl, pathName);
                    if (result.IsSuccess && result.Data != null)
                        _result.Messages.Add("Upload image to firebase successful");
                    await _unitOfWork.SaveChangesAsync();
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