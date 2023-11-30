using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
    public class AccountService : GenericBackEndService, Contracts.IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly TokenModel _loginResponse;
        private readonly IJwtGenerator _jwtGenerator;
        private AppActionResult _result;
        private IAccountRepository _accountRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            TokenModel loginResponse,
            IJwtGenerator jwtGenerator,
          IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IServiceProvider serviceProvider
            ) : base(serviceProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _loginResponse = loginResponse;
            _jwtGenerator = jwtGenerator;
            _result = new();
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppActionResult> AddAddress(DeliveryAddressDTO addressDto)
        {
            try
            {
                var deliveryAddressRepository = Resolve<IDeliveryAddressRepository>();
                var address = await deliveryAddressRepository.Insert(_mapper.Map<DeliveryAddress>(addressDto));
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);

            }
            finally
            {
                await _unitOfWork.SaveChangeAsync();
                _result.Messages.Add(Constant.ResponseMessage.CREATE_SUCCESSFUL);
            }
            return _result;
        }

        public async Task<AppActionResult> AddRole(string role)
        {
            try
            {
                var roleRepository = Resolve<IIdentityRoleRepository>();
                await roleRepository.Insert(new IdentityRole { Name = role });

            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            finally
            {
                await _unitOfWork.SaveChangeAsync();
                _result.Messages.Add(Constant.ResponseMessage.CREATE_SUCCESSFUL);
            }

            return _result;
        }

        public async Task<AppActionResult> AssignRole(string userId, string roleName)
        {
            try
            {
                var roleRepository = Resolve<IIdentityRoleRepository>();
                var role = await roleRepository.GetByExpression(r => r.Name.ToLower() == roleName.ToLower().Trim());
                bool isValid = true;
                if (await _accountRepository.GetById(userId) == null)
                {
                    isValid = false;
                    _result.Messages.Add(Constant.ResponseMessage.NOTFOUND(userId, "account"));
                }
                if (role == null)
                {
                    isValid = false;
                    _result.Messages.Add(Constant.ResponseMessage.NOTFOUND_BY_FIELDNAME(roleName, "account"));
                }
                if (isValid)
                {
                    var userRoleRepository = Resolve<IUserRoleRepository>();
                    await userRoleRepository.Insert(new IdentityUserRole<string> { UserId = userId, RoleId = role.Id });
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
                _result.Messages.Add(Constant.ResponseMessage.CREATE_SUCCESSFUL);
            }
            return _result;
        }

        public async Task<AppActionResult> DeleteAccount(ApplicationUser user)
        {
            try
            {
                await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
                _result.Messages.Add(Constant.ResponseMessage.DELETE_FAILED);

            }
            finally
            {
                _result.Messages.Add(Constant.ResponseMessage.DELETE_SUCCESSFUL);
            }
            return _result;
        }

        public async Task<AppActionResult> DeleteRole(IdentityRole roleDto)
        {
            try
            {
                var roleRepository = Resolve<IIdentityRoleRepository>();
                var role = await roleRepository.GetById(roleDto.Id);
                bool isValid = true;

                if (role == null)
                {
                    isValid = false;
                    _result.Messages.Add(Constant.ResponseMessage.NOTFOUND(roleDto.Id, "role"));
                }
                if (isValid)
                {
                    await _roleManager.DeleteAsync(role);
                }

            }
            catch (Exception ex)
            {

                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
                _result.Messages.Add(Constant.ResponseMessage.DELETE_FAILED);

            }
            finally
            {
                await _unitOfWork.SaveChangeAsync();
                _result.Messages.Add(Constant.ResponseMessage.DELETE_SUCCESSFUL);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAccountById(string accountId)
        {
            try
            {
                var account = await _accountRepository.GetById(accountId);
                if (account != null)
                {
                    _result.Data = account;
                }

            }
            catch (Exception ex)
            {
                _result.Messages.Add(ex.Message);
                _result.IsSuccess = false;
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllAccount()
        {
            try
            {
                _result.Data = await _accountRepository.GetAll();
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllAddressByUserId(string userId)
        {
            try
            {
                bool isValid = true;
                if (await _accountRepository.GetById(userId) == null)
                {
                    isValid = false;
                    _result.Messages.Add(Constant.ResponseMessage.NOTFOUND(userId, "account"));
                }
                if (isValid)
                {
                    var addressRepository = Resolve<IDeliveryAddressRepository>();
                    _result.Data = await addressRepository.GetByExpression(a => a.ApplicationUserId == userId);
                }

            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllRole()
        {
            try
            {
                var roleRepository = Resolve<IIdentityRoleRepository>();
                _result.Data = await roleRepository.GetAll();
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetRoleForUserId(string userId)
        {
            try
            {
                bool isValid = true;
                if (await _accountRepository.GetById(userId) == null)
                {
                    isValid = false;
                    _result.Messages.Add(Constant.ResponseMessage.NOTFOUND(userId, "account"));
                }
                if (isValid)
                {
                    var roleRepository = Resolve<IIdentityRoleRepository>();
                    _result.Data = await roleRepository.GetAll();

                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> Login(LoginRequest loginRequest)
        {
            try
            {

                var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);


                if (!result.Succeeded)
                {
                    _result.Messages.Add("The username or password is wrong");

                }
                string token = await _jwtGenerator.GenerateAccessToken(loginRequest);
                var user = await _accountRepository.GetByExpression(u => u.Email.ToLower() == loginRequest.Email.ToLower());
                string refreshToken = user.RefreshToken;

                if (user?.RefreshToken == null || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
                    user.RefreshToken = _jwtGenerator.GenerateRefreshToken();

                }
                _loginResponse.Token = token;
                _loginResponse.RefreshToken = refreshToken;
                _result.Data = _loginResponse;
            }
            catch (Exception e)
            {
            }
            finally
            {
                await _unitOfWork.SaveChangeAsync();

            }
            return _result;

        }

        public async Task<AppActionResult> RemoveAddress(DeliveryAddressDTO addressDto)
        {
            try
            {
                var addressRepository = Resolve<IDeliveryAddressRepository>();
                bool isValid = true;
                var address = await addressRepository.GetById(addressDto.DeliveryAddressId);
                if (address == null)
                {
                    isValid = false;
                    _result.Messages.Add(Constant.ResponseMessage.NOTFOUND(addressDto.DeliveryAddressId, "address"));
                }
                if (isValid)
                {
                    await addressRepository.DeleteById(addressDto.DeliveryAddressId);
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
            return _result;
        }

        public async Task<AppActionResult> SignUp(SignUpRequest dto)
        {

            try { } catch (Exception ex) { } finally { _unitOfWork.SaveChangeAsync(); }
            return _result;
        }

        public async Task<AppActionResult> UpdateAccount(ApplicationUser user)
        {

            try
            {
                bool isValid = true;
                var account = await _accountRepository.GetById(user.Id);
                if (account == null)
                {
                    isValid = false;
                    _result.Messages.Add(Constant.ResponseMessage.NOTFOUND(user.Id, "account"));
                }
                if (isValid)
                {
                    account = user;
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
            return _result;
        }

        public async Task<AppActionResult> UpdateAddress(DeliveryAddressDTO addressDto)
        {
            var addressRepository = Resolve<IDeliveryAddressRepository>();
            try
            {
                bool isValid = true;
                var address = await addressRepository.GetById(addressDto.DeliveryAddressId);
                if (address == null)
                {
                    isValid = false;
                    _result.Messages.Add(Constant.ResponseMessage.NOTFOUND(addressDto.DeliveryAddressId, "address"));
                }
                if (isValid)
                {
                    address = _mapper.Map<DeliveryAddress>(addressDto);
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
                _result.Messages.Add(Constant.ResponseMessage.UPDATE_SUCCESSFUL);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateRole(IdentityRole roleDto)
        {

            try
            {
                var roleRepository = Resolve<IIdentityRoleRepository>();
                bool isValid = true;
                var role = await roleRepository.GetById(roleDto.Id);
                if (role == null)
                {
                    isValid = false;
                    _result.Messages.Add(Constant.ResponseMessage.NOTFOUND(roleDto.Id, "Role"));
                }
                if (isValid)
                {
                    await _roleManager.UpdateAsync(roleDto);
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
            return _result;
        }
    }
}
