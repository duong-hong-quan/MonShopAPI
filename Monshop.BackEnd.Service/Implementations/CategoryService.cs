using AutoMapper;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.IRepository;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IUnitOfWork _unitOfWork;
        private AppActionResult _result;
        private IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _result = new();
            _mapper = mapper;
        }

        public async Task<AppActionResult> AddCategory(CategoryDTO dto)
        {
            try
            {
                bool isValid = true;
                await _unitOfWork.BeginTransaction();
                if (await _categoryRepository.GetByExpression(c => c.CategoryName == dto.CategoryName) != null)
                {
                    isValid = false;
                    _result.Messages.Add("The category with name is existed");
                }
                else if (isValid)
                {
                    await _categoryRepository.Insert(_mapper.Map<Category>(dto));
                    await _unitOfWork.SaveChangeAndCommitAsync();
                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);

            }
            return _result;
        }

        public async Task<AppActionResult> DeleteCategory(CategoryDTO dto)
        {
            try
            {
                _result.Messages.Add("Not implement");


            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllCategory()
        {
            try
            {
                _result.Data = await _categoryRepository.GetAll();
            }
            catch (Exception ex)
            {

                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateCategory(CategoryDTO dto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                await _categoryRepository.Update(_mapper.Map<Category>(dto));
                await _unitOfWork.SaveChangeAndCommitAsync();
            }
            catch (Exception ex)
            {

                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }
    }
}
