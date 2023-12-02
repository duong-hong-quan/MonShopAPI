using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.DAL.Models;
using Monshop.BackEnd.Service.Contracts;
using System.Security.Claims;

namespace MonShop.BackEnd.API.Controller
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            IAccountService accountService,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        [HttpPost("create-account")]

        public async Task<AppActionResult> CreateAccount(SignUpRequestDto request)
        {
            return await _accountService.CreateAccount(request, false);

        }

        [HttpPost("login")]
        public async Task<AppActionResult> Login(LoginRequestDto request)
        {
            return await _accountService.Login(request);

        }

        [HttpPut("update-account")]

        public async Task<AppActionResult> UpdateAccount(ApplicationUser request)
        {
            return await _accountService.UpdateAccount(request);

        }
        [HttpGet("get-account-by-id/{id}")]

        public async Task<AppActionResult> UpdateAccount(string id)
        {
            return await _accountService.GetAccountByUserId(id);

        }

        [HttpPost("get-all-account")]
        //  [Authorize(Roles = Permission.MANAGEMENT)]

        public async Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            return await _accountService.GetAllAccount(pageIndex, pageSize, sortInfos);
        }
        [HttpPut("change-password")]

        public async Task<AppActionResult> ChangePassword(ChangePasswordDto dto)
        {
            return await _accountService.ChangePassword(dto);
        }

        [HttpPost("get-accounts-with-searching")]
        public async Task<AppActionResult> GetAccountWithSearching(BaseFilterRequest baseFilterRequest)
        {
            return await _accountService.SearchApplyingSortingAndFiltering(baseFilterRequest);
        }

        [HttpPut("assign-role-for-userId")]

        public async Task<AppActionResult> AssignRoleForUserId(string userId, IList<string> roleId)
        {
            return await _accountService.AssignRoleForUserId(userId, roleId);
        }
        [HttpPut("remove-role-for-userId")]

        public async Task<AppActionResult> RemoveRoleForUserId(string userId, IList<string> roleId)
        {
            return await _accountService.RemoveRoleForUserId(userId, roleId);
        }


        [HttpPost("get-new-token")]
        public async Task<AppActionResult> GetNewToken([FromBody] string refreshToken, string userId)
        {
            return await _accountService.GetNewToken(refreshToken, userId);

        }
        [HttpPut("forgot-password")]

        public async Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            return await _accountService.ForgotPassword(dto);
        }
        [HttpPut("active-account")]

        public async Task<AppActionResult> ActiveAccount(string email, string verifyCode)
        {

            return await _accountService.ActiveAccount(email, verifyCode);
        }
        [HttpPost("send-email-forgot-password/{email}")]

        public async Task<AppActionResult> SendEmailForgotPassword(string email)
        {
            return await _accountService.SendEmailForgotPassword(email);
        }




        [HttpGet("login-google")]
        public IActionResult ExternalLogin()
        {

            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = _configuration["Domain:FE"] });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [AllowAnonymous]
        [HttpGet("external-login-google-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Redirect($"{returnUrl}/failed"); ;
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                string code = await _accountService.GenerateVerifyCode(email);
                if (code == string.Empty)
                {
                    SignUpRequestDto requestDto = new SignUpRequestDto()
                    {
                        Email = email,
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                        Password = "Abc123@",
                        RoleName = new List<string>() { "trainee" }
                    };
                    await _accountService.CreateAccount(requestDto, true);
                    code = await _accountService.GenerateVerifyCodeGoogle(email);
                    await _accountService.ActiveAccount(email, code);
                }
                return Redirect($"{returnUrl}/success/{email}/{code}");
            }
        }
        [AllowAnonymous]
        [HttpPost("verify-login-google/{email}/{verifyCode}")]
        public async Task<AppActionResult> VerifyLoginGoogle(string email, string verifyCode)
        {
            return await _accountService.VerifyLoginGoogle(email, verifyCode);
        }
       


    }
}
