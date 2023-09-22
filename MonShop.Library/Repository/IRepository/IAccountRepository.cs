﻿using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.Library.DTO;
using Microsoft.AspNetCore.Identity;

namespace MonShop.Library.Repository.IRepository
{
    public interface IAccountRepository
    {
        public Task<string> Login(LoginRequest loginRequest);
        public Task SignUp(SignUpRequest dto);
        public  Task UpdateAccount(ApplicationUser user);
        public Task DeleteAccount(ApplicationUser user);
        public Task<ApplicationUser> GetAccountById(string accountId);
        public Task<IEnumerable<ApplicationUser>> GetAllAccount();
        public Task AssignRole(string userId, string roleName);
        public Task<IEnumerable<IdentityRole>> GetAllRole();
        public Task AddRole(string role);
        public  Task UpdateRole(IdentityRole roleDto);
        public Task DeleteRole(IdentityRole roleDto);
        public Task<IdentityRole<string>> GetRoleForUserId(string userId);


    }
}
