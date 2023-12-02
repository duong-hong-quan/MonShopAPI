using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.DAL.Models;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface IAccountService 
    {
        Task<AppActionResult> Login(LoginRequestDto loginRequest);
        public Task<AppActionResult> VerifyLoginGoogle(string email, string verifyCode);

        Task<AppActionResult> CreateAccount(SignUpRequestDto signUpRequest, bool isGoogle);
        Task<AppActionResult> UpdateAccount(ApplicationUser applicationUser);
        Task<AppActionResult> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<AppActionResult> GetAccountByUserId(string id);
        Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize, IList<SortInfo> sortInfos);
        Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest);
        Task<AppActionResult> AssignRoleForUserId(string userId, IList<string> roleId);
        Task<AppActionResult> RemoveRoleForUserId(string userId, IList<string> roleId);
        Task<AppActionResult> GetNewToken(string refreshToken, string userId);
        Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto);
        Task<AppActionResult> ActiveAccount(string email, string verifyCode);
        Task<AppActionResult> SendEmailForgotPassword(string email);
        Task<string> GenerateVerifyCode(string email);
        Task<string> GenerateVerifyCodeGoogle(string email);


    }
}
