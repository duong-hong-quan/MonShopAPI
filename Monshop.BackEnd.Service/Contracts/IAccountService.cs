using Microsoft.AspNetCore.Identity;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface IAccountService
    {
        public Task<AppActionResult> Login(LoginRequest loginRequest);
        public Task<AppActionResult> SignUp(SignUpRequest dto);
        public Task<AppActionResult> UpdateAccount(ApplicationUser user);
        public Task<AppActionResult> DeleteAccount(ApplicationUser user);
        public Task<AppActionResult> GetAccountById(string accountId);
        public Task<AppActionResult> GetAllAccount();
        public Task<AppActionResult> AssignRole(string userId, string roleName);
        public Task<AppActionResult> GetAllRole();
        public Task<AppActionResult> AddRole(string role);
        public Task<AppActionResult> UpdateRole(IdentityRole roleDto);
        public Task<AppActionResult> DeleteRole(IdentityRole roleDto);
        public Task<AppActionResult> GetRoleForUserId(string userId);
        public Task<AppActionResult> AddAddress(DeliveryAddressDTO addressDto);
        public Task<AppActionResult> UpdateAddress(DeliveryAddressDTO addressDto);
        public Task<AppActionResult> RemoveAddress(DeliveryAddressDTO addressDto);
        public Task<AppActionResult> GetAllAddressByUserId(string userId);
    }
}
