﻿using Microsoft.AspNetCore.Authorization;
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
        

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ResponseDTO> Login(LoginRequest userLogin)
        {
            try
            {
                var token = await _accountRepository.Login(userLogin);
                if (token != string.Empty)
                {
                  
                    _response.Data = token;
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
       

    }

}
