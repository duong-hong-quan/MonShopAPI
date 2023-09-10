using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MonShopAPI.Util;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using MonShopLibrary.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MonShop.Library.DTO;
using MonShopLibrary.Utils;
using MonShop.Controller.Model;

namespace MonShopAPI.Controller
{
    [Route("Account")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ResponeDTO _responeDTO;
        private readonly LoginRespone _loginRespone;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _responeDTO = new ResponeDTO();
            _loginRespone = new LoginRespone();

        }
        [HttpGet]
        [Route("GetAllAccount")]
        public async Task<ResponeDTO> GetAllAccount()
        {
            try
            {

                _responeDTO.Data = await _accountRepository.GetAllAccount();


            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;

            }
            return _responeDTO;
        }
        [HttpGet]
        [Route("GetAllRole")]
        public async Task<ResponeDTO> GetAllRole()
        {
            try
            {
                _responeDTO.Data = await _accountRepository.GetAllRole();


            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
        [HttpPost]
        [Route("AddAccount")]
        public async Task<IActionResult> Add(AccountDTO dto)
        {
            await _accountRepository.AddAccount(dto);
            return Ok("Add account successfully");
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult> Update(AccountDTO dto)
        {
            await _accountRepository.UpdateAccount(dto);
            return Ok("Update account successfully");
        }

        [HttpDelete]
        [Route("DeleteAccount")]
        public async Task<IActionResult> Delete(AccountDTO dto)
        {
            await _accountRepository.DeleteAccount(dto);
            return Ok("Delete account successfully");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ResponeDTO> Login(LoginRequest userLogin)
        {
            try
            {
                var user = await _accountRepository.Login(userLogin);
                if (user != null)
                {
                    var token = JWTHelper.GenerateToken(user);

                    _loginRespone.Token = token;
                    _loginRespone.RefreshToken = await _accountRepository.GenerateRefreshToken(user.AccountId);
                    _responeDTO.Data = _loginRespone;
                }


            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;

            }




            return _responeDTO;
        }

        [HttpGet]
        [Route("GetNewToken")]

        public async Task<ResponeDTO> GetNewToken(string refreshToken)
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
                        _responeDTO.Data = _loginRespone;

                    }
                }

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;

            }



            return _responeDTO;

        }

        [HttpGet]
        [Route("GetAccountByID")]
        public async Task<ResponeDTO> GetAccountByID(int AccountID)
        {
            try
            {
                _responeDTO.Data = await _accountRepository.GetAccountByID(AccountID);

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;

        }
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(AccountDTO accountDTO)
        {
            await _accountRepository.SignUp(accountDTO);
            return Ok("Sign up successfully !");
        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            Account account = await _accountRepository.GetAccountByID(request.AccountId);
            if (account == null)
            {

                return BadRequest($"No result for account with ID {request.AccountId}");
            }
            else
            {
                bool check = MonShopLibrary.Utils.Utility.VerifyPassword(request.OldPassword, account.Password);
                if (check)
                {
                    await _accountRepository.ChangePassword(request);
                    return Ok();
                }
                else
                {
                    return BadRequest("The old password is wrong!");

                }
            }


        }

    }

}
