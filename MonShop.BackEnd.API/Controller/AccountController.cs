using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.DTO.Response;
using Monshop.BackEnd.Service.Contracts;

namespace MonShop.BackEnd.API.Controller
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;

        }

        [HttpPost]
        [Route("Login")]
        public async Task<AppActionResult> Login(LoginRequest userLogin)
        {
            return await _accountService.Login(userLogin);
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<AppActionResult> SignUp(SignUpRequest accountDTO)
        {
            return await _accountService.SignUp(accountDTO);
        }
        [Authorize]
        [HttpPut("UpdateAccount")]
        public async Task<AppActionResult> UpdateAccount(ApplicationUser userDto)
        {
            return await _accountService.UpdateAccount(userDto);
        }
        [Authorize]
        [HttpDelete("DeleteAccount")]
        public async Task<AppActionResult> DeleteAccount(ApplicationUser user)
        {
            return await _accountService.DeleteAccount(user);
        }


        [HttpGet("GetAccountById/{accountId}")]
        public async Task<AppActionResult> GetAccountById(string accountId)
        {
            return await _accountService.GetAccountById(accountId);
        }

        [Authorize]
        [HttpGet("GetAllAccount")]
        public async Task<AppActionResult> GetAllAccount()
        {
            return await _accountService.GetAllAccount();
        }

        [HttpPost("AssignRole")]
        public async Task<AppActionResult> AssignRole(string userId, string roleName)
        {
           return await _accountService.AssignRole(userId, roleName);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllRole")]
        public async Task<AppActionResult> GetAllRole()
        {
            return await _accountService.GetAllRole();
        }

        [HttpPost("AddRole")]
        public async Task<AppActionResult> AddRole(string role)
        {
          return await _accountService.AddRole(role);
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateRole")]
        public async Task<AppActionResult> UpdateRole(IdentityRole roleDto)
        {
            return await _accountService.UpdateRole(roleDto);
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteRole")]
        public async Task<AppActionResult> DeleteRole(IdentityRole roleDto)
        {
            
            return await _accountService.DeleteRole(roleDto);
        }
        [HttpGet("GetRoleForUserId/{accountId}")]
        public async Task<AppActionResult> GetRoleForUserId(string accountId)
        {
           return await _accountService.GetRoleForUserId(accountId);
        }


       
    }

}
