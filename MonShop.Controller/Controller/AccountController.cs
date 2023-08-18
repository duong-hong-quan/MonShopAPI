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

namespace MonShopAPI.Controller
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController()
        {
            _accountRepository = new AccountRepository();

        }
        [HttpGet]
        [Route("GetAllAccount")]
        public async Task<IActionResult> GetAllAccount()
        {
            var list = await _accountRepository.GetAllAccount();
            return Ok(list);
        }
        [HttpGet]
        [Route("GetAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            var list = await _accountRepository.GetAllRole();
            return Ok(list);
        }
        [HttpPost]
        [Route("AddAccount")]
        public async Task<IActionResult> Add(AccountDTO dto)
        {
            await _accountRepository.AddAccount(dto);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult> Update(AccountDTO dto)
        {
            await _accountRepository.UpdateAccount(dto);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteAccount")]
        public async Task<IActionResult> Delete(AccountDTO dto)
        {
            await _accountRepository.DeleteAccount(dto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginRequest userLogin)
        {
            var user = await _accountRepository.Login(userLogin);
            if (user != null)
            {
                var token = JWTHelper.GenerateToken(user);
                return Ok(new LoginRespone
                {
                    Token = token,
                    RefreshToken = await _accountRepository.GenerateRefreshToken(user.AccountId)
                });
            }

            return NotFound("User not found");
        }

        [HttpGet]
        [Route("GetNewToken")]

        public async Task<IActionResult> GetNewToken(string refreshToken)
        {
            Token token = await _accountRepository.GetToken(refreshToken);
            if(token != null)
            {
                Account account = await _accountRepository.GetAccountByID(token.AccountId);
                if (account != null)
                {
                    var newToken = JWTHelper.GenerateToken(account);
                    return Ok(new LoginRespone
                    {
                        Token = newToken,
                        RefreshToken = await _accountRepository.GenerateRefreshToken(token.AccountId)
                    });
                }
            }

          
            return BadRequest($"User not found");

        }

        [HttpGet]
        [Route("GetAccountByID")]
        public async Task<IActionResult> GetAccountByID(int AccountID)
        {
            Account account = await _accountRepository.GetAccountByID(AccountID);
            return Ok(account);

        }

      

    }

}
