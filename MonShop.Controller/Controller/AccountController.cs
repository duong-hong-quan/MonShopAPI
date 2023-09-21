using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MonShopAPI.Util;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MonShop.Library.DTO;
using MonShopLibrary.Utils;
using MonShop.Controller.Model;
using MonShop.Library.Repository.IRepository;

namespace MonShopAPI.Controller
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ResponseDTO _response;
        private readonly LoginResponse _loginRespone;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _response = new ResponseDTO();
            _loginRespone = new LoginResponse();

        }
        [HttpGet]
        [Route("GetAllAccount")]
        public async Task<ResponseDTO> GetAllAccount()
        {
            try
            {
                _response.Data = await _accountRepository.GetAllAccount();

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }
        [HttpGet]
        [Route("GetAllRole")]
        public async Task<ResponseDTO> GetAllRole()
        {
            try
            {
                _response.Data = await _accountRepository.GetAllRole();


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPost]
        [Route("AddAccount")]
        public async Task<ResponseDTO> Add(AccountDTO dto)
        {
            try
            {

                await _accountRepository.AddAccount(dto);
                _response.Message = "Add account successfully";
                _response.Data = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<ResponseDTO> Update(AccountDTO dto)
        {
            try
            {
                await _accountRepository.UpdateAccount(dto);

                _response.Message = "Update account successfully";
                _response.Data = true;
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpDelete]
        [Route("DeleteAccount")]
        public async Task<ResponseDTO> Delete(AccountDTO dto)
        {
            try
            {
                await _accountRepository.DeleteAccount(dto);
                _response.Message = "Delete account successfully";
                _response.Data = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ResponseDTO> Login(LoginRequest userLogin)
        {
            try
            {
                var user = await _accountRepository.Login(userLogin);
                if (user != null)
                {
                    var token = JWTHelper.GenerateToken(user);

                    _loginRespone.Token = token;
                    _loginRespone.RefreshToken = await _accountRepository.GenerateRefreshToken(user.AccountId);
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

        [HttpGet]
        [Route("GetNewToken/{refreshToken}")]

        public async Task<ResponseDTO> GetNewToken(string refreshToken)
        {
            try
            {

                Token token = await _accountRepository.GetToken(refreshToken);
                if (token != null)
                {
                    Account account = await _accountRepository.GetAccountByID(token.AccountId);
                    if (account != null)
                    {
                        var newToken = JWTHelper.GenerateToken(account);

                        _loginRespone.Token = newToken;
                        _loginRespone.RefreshToken = await _accountRepository.GenerateRefreshToken(token.AccountId);
                        _response.Data = _loginRespone;

                    }
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }



            return _response;

        }

        [HttpGet]
        [Route("GetAccountByID/{AccountID}")]
        public async Task<ResponseDTO> GetAccountByID(int AccountID)
        {
            try
            {
                _response.Data = await _accountRepository.GetAccountByID(AccountID);

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
        public async Task<ResponseDTO> SignUp(AccountDTO accountDTO)
        {
            try
            {
                await _accountRepository.SignUp(accountDTO);
                _response.Data = true;
                _response.Message = "Sign up successfully !";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ResponseDTO> ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                Account account = await _accountRepository.GetAccountByID(request.AccountId);
                if (account == null)
                {
                    _response.Message = $"No result for account with ID {request.AccountId}";
                    _response.IsSuccess = false;
                    return _response;
                }
                else
                {
                    bool check = MonShopLibrary.Utils.Utility.VerifyPassword(request.OldPassword, account.Password);
                    if (check)
                    {
                        await _accountRepository.ChangePassword(request);
                        _response.Data = true;
                    }
                    else
                    {
                        _response.Message = "The old password is wrong!";
                        _response.IsSuccess = false;
                        return _response;

                    }
                }
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
