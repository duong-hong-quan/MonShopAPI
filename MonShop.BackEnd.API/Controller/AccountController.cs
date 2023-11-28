using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MonShop.BackEnd.Utility.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using MonShop.BackEnd.API.Model;
using MonShop.BackEnd.DAL.Repository.IRepository;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;

namespace MonShop.BackEnd.API.Controller
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ResponseDTO _response;
        private TokenModel _loginRespone;
        private readonly IJwtGenerator _jwtGenerator;
        public AccountController(IAccountRepository accountRepository, IJwtGenerator jwtGenerator)
        {
            _accountRepository = accountRepository;
            _response = new ResponseDTO();
            _loginRespone = new TokenModel();
            _jwtGenerator = jwtGenerator;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ResponseDTO> Login(LoginRequest userLogin)
        {
            try
            {
                _loginRespone = await _accountRepository.Login(userLogin);
                if (_loginRespone.Token != null)
                {

                    _response.Data = _loginRespone;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<ResponseDTO> SignUp(SignUpRequest accountDTO)
        {
            try
            {
                _response.Data = await _accountRepository.SignUp(accountDTO);
                _response.Message = "Sign up sucessfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }
        [Authorize]
        [HttpPut("UpdateAccount")]
        public async Task<ResponseDTO> UpdateAccount(ApplicationUser userDto)
        {
            try
            {
                await _accountRepository.UpdateAccount(userDto);
                _response.Data = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [Authorize]
        [HttpDelete("DeleteAccount")]
        public async Task<ResponseDTO> DeleteAccount(ApplicationUser user)
        {
            try
            {
                await _accountRepository.DeleteAccount(user);
                _response.Data = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }


        [HttpGet("GetAccountById/{accountId}")]
        public async Task<ResponseDTO> GetAccountById(string accountId)
        {
            try
            {
                _response.Data = await _accountRepository.GetAccountById(accountId);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [Authorize]
        [HttpGet("GetAllAccount")]
        public async Task<ResponseDTO> GetAllAccount()
        {
            try
            {
                var list = await _accountRepository.GetAllAccount();
                _response.Data = list;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("AssignRole")]
        public async Task<ResponseDTO> AssignRole(string userId, string roleName)
        {
            try
            {
                await _accountRepository.AssignRole(userId, roleName);
                _response.Data = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllRole")]
        public async Task<ResponseDTO> GetAllRole()
        {
            try
            {
                var list = await _accountRepository.GetAllRole();
                _response.Data = list;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("AddRole")]
        public async Task<ResponseDTO> AddRole(string role)
        {
            try
            {
                await _accountRepository.AddRole(role);
                _response.Data = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateRole")]
        public async Task<ResponseDTO> UpdateRole(IdentityRole roleDto)
        {
            try
            {
                await _accountRepository.UpdateRole(roleDto);
                _response.Data = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteRole")]
        public async Task<ResponseDTO> DeleteRole(IdentityRole roleDto)
        {
            try
            {
                await _accountRepository.DeleteRole(roleDto);
                _response.Data = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet("GetRoleForUserId/{accountId}")]
        public async Task<ResponseDTO> GetRoleForUserId(string accountId)
        {
            try
            {
                var list = await _accountRepository.GetRoleForUserId(accountId);
                _response.Data = list;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpGet("GetNewToken")]
        public async Task<ResponseDTO> GetNewToken(string refreshToken, string accountId)
        {
            try
            {
                var token = await _jwtGenerator.GetNewToken(refreshToken, accountId);
                _loginRespone = token;
                _response.Data = _loginRespone;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }

}
