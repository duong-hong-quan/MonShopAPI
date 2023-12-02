using Microsoft.AspNetCore.Identity;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.Common.Dto.Response;
using MonShop.BackEnd.Utility.Utils;
using MonShop.BackEnd.DAL.Utils;
using MonShop.BackEnd.DAL.Common;

namespace Monshop.BackEnd.Service.Implementations
{
    public class AccountService : GenericBackendService, IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppActionResult _result;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenDto _tokenDto;
        private IAccountRepository _accountRepository;
        public AccountService(
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
           IServiceProvider serviceProvider

            ) : base(serviceProvider)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _result = new();
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenDto = new();
        }

        public async Task<AppActionResult> Login(LoginRequestDto loginRequest)
        {
            bool isValid = true;
            try
            {
                var user = await _accountRepository.GetByExpression(u => u.Email.ToLower() == loginRequest.Email.ToLower() && u.IsDeleted == false);
                if (user == null)
                {
                    isValid = false;
                    _result.Messages.Add($"The user with username {loginRequest.Email} not found");
                }
                else if (user.IsVerified == false)
                {
                    isValid = false;
                    _result.Messages.Add("The account is not verified !");
                }

                var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);
                if (!result.Succeeded)
                {
                    isValid = false;
                    _result.Messages.Add(SD.ResponseMessage.LOGIN_FAILED);
                }

                if (isValid)
                {
                    await LoginDefault(loginRequest.Email, user);

                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }

            return _result;

        }
        public async Task<AppActionResult> VerifyLoginGoogle(string email, string verifyCode)
        {
            bool isValid = true;
            try
            {
                var user = await _accountRepository.GetByExpression(u => u.Email.ToLower() == email.ToLower() && u.IsDeleted == false);
                if (user == null)
                {
                    isValid = false;
                    _result.Messages.Add($"The user with username {email} not found");
                }
                else if (user.IsVerified == false)
                {
                    isValid = false;
                    _result.Messages.Add("The account is not verified !");
                }
                else if (user.VerifyCode != verifyCode)
                {
                    isValid = false;
                    _result.Messages.Add("The  verify code is wrong !");

                }

                if (isValid)
                {
                    await LoginDefault(email, user);
                    user.VerifyCode = null;
                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            finally
            {
                await _unitOfWork.SaveChangeAsync();

            }

            return _result;

        }

        private async Task LoginDefault(string email, ApplicationUser? user)
        {

            var jwtService = Resolve<IJwtService>();
            var utility = Resolve<Utility>();
            string token = await jwtService.GenerateAccessToken(new LoginRequestDto { Email = email });

            if (user.RefreshToken == null)
            {
                user.RefreshToken = jwtService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = utility.GetCurrentDateInTimeZone().AddDays(1);
            }
            if (user.RefreshTokenExpiryTime <= utility.GetCurrentDateInTimeZone())
            {
                user.RefreshTokenExpiryTime = utility.GetCurrentDateInTimeZone().AddDays(1);
                user.RefreshToken = jwtService.GenerateRefreshToken();
            }

            await _unitOfWork.SaveChangeAsync();
            _tokenDto.Token = token;
            _tokenDto.RefreshToken = user.RefreshToken;
            _result.Result.Data = _tokenDto;
        }

        public async Task<AppActionResult> CreateAccount(SignUpRequestDto signUpRequest, bool isGoogle)
        {
            bool isValid = true;
            var userRoleRepository = Resolve<IUserRoleRepository>();
            var identityRoleRepository = Resolve<IIdentityRoleRepository>();
            try
            {
                if (await _accountRepository.GetByExpression(r => r.UserName == signUpRequest.Email) != null)
                {
                    _result.Messages.Add("The email or username is existed");
                    isValid = false;

                }
                foreach (var role in signUpRequest.RoleName)
                {
                    if (await identityRoleRepository.GetById(role) == null)
                    {
                        _result.Messages.Add($"The role with id {role} is not existed");
                    }
                }
                if (isValid)
                {
                    var emailService = Resolve<IEmailService>();
                    string verifyCode = null;
                    if (!isGoogle)
                    {
                        verifyCode = Guid.NewGuid().ToString("N").Substring(0, 6);

                    }

                    var user = new ApplicationUser
                    {
                        Email = signUpRequest.Email,
                        UserName = signUpRequest.Email,
                        FirstName = signUpRequest.FirstName,
                        LastName = signUpRequest.LastName,
                        PhoneNumber = signUpRequest.PhoneNumber,
                        Gender = signUpRequest.Gender,
                        VerifyCode = verifyCode,
                        IsVerified = isGoogle ? true : false


                    };
                    var resultCreateUser = await _userManager.CreateAsync(user, signUpRequest.Password);
                    if (resultCreateUser.Succeeded)
                    {
                        _result.Result.Data = user;
                        _result.Messages.Add($"{SD.ResponseMessage.CREATE_SUCCESSFUL} USER");
                        if (!isGoogle)
                        {
                            emailService.SendEmail(user.Email, SD.SubjectMail.VERIFY_ACCOUNT, verifyCode);
                        }

                    }
                    else
                    {
                        _result.Messages.Add($"{SD.ResponseMessage.CREATE_FAILED} USER");

                    }

                    foreach (var role in signUpRequest.RoleName)
                    {
                        var roleDB = await identityRoleRepository.GetByExpression(r => r.Name.ToLower() == role.ToLower());
                        var resultCreateRole = await _userManager.AddToRoleAsync(user, roleDB.NormalizedName);
                        if (resultCreateRole.Succeeded)
                        {
                            _result.Messages.Add($"ASSIGN ROLE SUCCESSFUL");

                        }
                        else
                        {
                            _result.Messages.Add($"ASSIGN ROLE FAILED");

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                _result.Messages.Add(ex.Message);
                _result.IsSuccess = false;
            }
            finally
            {
                await _unitOfWork.SaveChangeAsync();

            }
            return _result;
        }
        public async Task<AppActionResult> UpdateAccount(ApplicationUser applicationUser)
        {
            bool isValid = true;
            try
            {

                if (await _accountRepository.GetByExpression(a => a.Id == applicationUser.Id) == null)
                {
                    isValid = false;
                    _result.Messages.Add($"The user with id {applicationUser.Id} not found");
                }
                if (isValid)
                {
                    await _accountRepository.Update(applicationUser);
                    await _unitOfWork.SaveChangeAsync();
                    _result.Messages.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);

            }

            return _result;
        }
        public async Task<AppActionResult> GetAccountByUserId(string id)
        {
            bool isValid = true;
            try
            {

                if (await _accountRepository.GetById(id) == null)
                {
                    isValid = false;
                    _result.Messages.Add($"The user with id {id} not found");

                }
                if (isValid)
                {
                    _result.Result.Data = await _accountRepository.GetById(id);

                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }
        public async Task<AppActionResult> GetAllAccount(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                var userRoleRepository = Resolve<IUserRoleRepository>();
                var identityRoleRepository = Resolve<IIdentityRoleRepository>();
                List<AccountResponse> accounts = new List<AccountResponse>();
                var list = await _accountRepository.GetAll();
                if (pageIndex <= 0) pageIndex = 1;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(list.Count(), pageSize);

                foreach (var account in list)
                {
                   var userRole = await userRoleRepository.GetListByExpression(s => s.UserId == account.Id);
 var listRole = new List<IdentityRole>();
                    if (userRole.Any())
                    {
                        foreach (var role in userRole)
                        {
                            var item = await identityRoleRepository.GetById(role.RoleId);
                            listRole.Add(item);
                        }
                        accounts.Add(new AccountResponse { User = account, Role = listRole });
                    }
                    // accounts.Add(new AccountResponse { User = account, Role = listRole });

                }
                var data = accounts.AsQueryable().OrderBy(x => x.User.Id);
                if (sortInfos != null)
                {
                    data = DataPresentationHelper.ApplySorting(data, sortInfos);
                }
                if (pageIndex > 0 && pageSize > 0)
                {
                    data = DataPresentationHelper.ApplyPaging(data, pageIndex, pageSize);
                }
                _result.Result.Data = data;
                _result.Result.TotalPage = totalPage;

            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }
        public async Task<AppActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            bool isValid = true;
            try
            {
                if (await _accountRepository.GetByExpression(c => c.Email == changePasswordDto.Email && c.IsDeleted == false) == null)
                {
                    isValid = false;
                    _result.Messages.Add($"The user with email {changePasswordDto.Email} not found");
                }
                if (isValid)
                {
                    var user = await _accountRepository.GetByExpression(c => c.Email == changePasswordDto.Email && c.IsDeleted == false);
                    var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
                    if (result.Succeeded)
                    {
                        _result.Messages.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
                    }
                    else
                    {
                        _result.Messages.Add(SD.ResponseMessage.CREATE_FAILED);
                    }
                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }
        public async Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest)
        {
            try
            {
                var source = (IOrderedQueryable<ApplicationUser>)await _accountRepository.GetByExpression(a => (bool)a.IsDeleted, null);
                int pageSize = filterRequest.pageSize;
                if (filterRequest.pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), pageSize);
                if (filterRequest != null)
                {
                    if (filterRequest.pageIndex <= 0 || filterRequest.pageSize <= 0)
                    {
                        _result.Messages.Add($"Invalid value of pageIndex or pageSize");
                        _result.IsSuccess = false;
                    }
                    else
                    {
                        if (filterRequest.keyword != "")
                        {
                            source = (IOrderedQueryable<ApplicationUser>)await _accountRepository.GetByExpression(c => (bool)!c.IsDeleted && c.UserName.Contains(filterRequest.keyword), null);
                        }
                        if (filterRequest.filterInfoList != null)
                        {
                            source = DataPresentationHelper.ApplyFiltering(source, filterRequest.filterInfoList);
                        }
                        totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), filterRequest.pageSize);
                        if (filterRequest.sortInfoList != null)
                        {
                            source = DataPresentationHelper.ApplySorting(source, filterRequest.sortInfoList);
                        }
                        source = DataPresentationHelper.ApplyPaging(source, filterRequest.pageIndex, filterRequest.pageSize);
                        _result.Result.Data = source;

                    }
                }
                else
                {
                    _result.Result.Data = source;
                }
                _result.Result.TotalPage = totalPage;
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> AssignRoleForUserId(string userId, IList<string> roleId)
        {
            try
            {
                bool isValid = true;
                var user = await _accountRepository.GetById(userId);
                var userRoleRepository = Resolve<IUserRoleRepository>();
                var identityRoleRepository = Resolve<IIdentityRoleRepository>();
                if (user == null)
                {
                    isValid = false;
                    _result.Messages.Add($"The user with id {userId} is not existed");
                }
                foreach (var role in roleId)
                {
                    if (await identityRoleRepository.GetById(role) == null)
                    {
                        _result.Messages.Add($"The role with id {role} is not existed");
                    }
                }

                if (isValid)
                {
                    foreach (var role in roleId)
                    {
                        var roleDB = await identityRoleRepository.GetById(role);
                        var resultCreateRole = await _userManager.AddToRoleAsync(user, roleDB.NormalizedName);
                        if (resultCreateRole.Succeeded)
                        {
                            _result.Messages.Add($"ASSIGN ROLE {role} SUCCESSFUL");

                        }
                        else
                        {
                            _result.Messages.Add($"ASSIGN ROLE {role}  FAILED");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);

            }
            return _result;
        }

        public async Task<AppActionResult> RemoveRoleForUserId(string userId, IList<string> roleId)
        {
            try
            {
                bool isValid = true;
                var user = await _accountRepository.GetById(userId);
                var userRoleRepository = Resolve<IUserRoleRepository>();
                var identityRoleRepository = Resolve<IIdentityRoleRepository>();
                if (user == null)
                {
                    isValid = false;
                    _result.Messages.Add($"The user with id {userId} is not existed");
                }
                foreach (var role in roleId)
                {
                    if (await identityRoleRepository.GetById(role) == null)
                    {
                        _result.Messages.Add($"The role with id {role} is not existed");
                    }
                }

                if (isValid)
                {
                    foreach (var role in roleId)
                    {
                        var roleDB = await identityRoleRepository.GetById(role);
                        var resultCreateRole = await _userManager.RemoveFromRoleAsync(user, roleDB.NormalizedName);
                        if (resultCreateRole.Succeeded)
                        {
                            _result.Messages.Add($"REMOVE ROLE {role} SUCCESSFUL");

                        }
                        else
                        {
                            _result.Messages.Add($"REMOVE ROLE {role}  FAILED");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);

            }
            return _result;
        }


        public async Task<AppActionResult> GetNewToken(string refreshToken, string userId)
        {
            try
            {
                bool isValid = true;
                var user = await _accountRepository.GetById(userId);
                if (user == null)
                {
                    isValid = false;
                    _result.Messages.Add("The user is not existed");

                }
                else if (user.RefreshToken != refreshToken)
                {
                    isValid = false;
                    _result.Messages.Add("The refresh token is not exacted");
                }

                if (isValid)
                {
                    var jwtService = Resolve<JwtService>();
                    _result.Result.Data = await jwtService.GetNewToken(refreshToken, userId);

                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            try
            {
                bool isValid = true;
                var user = await _accountRepository.GetByExpression(a => a.Email == dto.Email && a.IsDeleted == false && a.IsVerified == true);
                if (user == null)
                {
                    isValid = false;
                    _result.Messages.Add("The user is not existed or is not verified");
                }
                else if (user.VerifyCode != dto.RecoveryCode)
                {
                    isValid = false;
                    _result.Messages.Add("The verification code is wrong.");

                }

                if (isValid)
                {
                    await _userManager.RemovePasswordAsync(user);
                    var result = await _userManager.AddPasswordAsync(user, dto.NewPassword);
                    if (result.Succeeded)
                    {
                        user.VerifyCode = null;
                        _result.Messages.Add("Change password successful");
                    }
                    else
                    {
                        _result.Messages.Add("Change password failed");

                    }
                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            finally
            {
                await _unitOfWork.SaveChangeAsync();
            }
            return _result;
        }

        public async Task<AppActionResult> ActiveAccount(string email, string verifyCode)
        {
            try
            {
                bool isValid = true;
                var user = await _accountRepository.GetByExpression(a => a.Email == email && a.IsDeleted == false && a.IsVerified == false);
                if (user == null)
                {
                    isValid = false;
                    _result.Messages.Add("The user is not existed ");
                }
                else if (user.VerifyCode != verifyCode)
                {
                    isValid = false;
                    _result.Messages.Add("The verification code is wrong.");

                }

                if (isValid)
                {
                    user.IsVerified = true;
                    user.VerifyCode = null;
                    await _unitOfWork.SaveChangeAsync();
                    _result.Messages.Add("Active successfully");
                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> SendEmailForgotPassword(string email)
        {
            try
            {
                bool isValid = true;
                var user = await _accountRepository.GetByExpression(a => a.Email == email && a.IsDeleted == false && a.IsVerified == true);
                if (user == null)
                {
                    isValid = false;
                    _result.Messages.Add("The user is not existed or is not verified");
                }


                if (isValid)
                {
                    var emailService = Resolve<IEmailService>();
                    string code = await GenerateVerifyCode(user.Email);
                    emailService.SendEmail(email, SD.SubjectMail.PASSCODE_FORGOT_PASSWORD, code);

                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<string> GenerateVerifyCode(string email)
        {
            var user = await _accountRepository.GetByExpression(a => a.Email == email && a.IsDeleted == false && a.IsVerified == true);

            if (user != null)
            {
                string code = Guid.NewGuid().ToString("N").Substring(0, 6);
                user.VerifyCode = code;
                await _unitOfWork.SaveChangeAsync();
                return code;

            }
            return string.Empty;
        }
        public async Task<string> GenerateVerifyCodeGoogle(string email)
        {
            var user = await _accountRepository.GetByExpression(a => a.Email == email && a.IsDeleted == false);

            if (user != null)
            {
                string code = Guid.NewGuid().ToString("N").Substring(0, 6);
                user.VerifyCode = code;
                await _unitOfWork.SaveChangeAsync();
                return code;

            }
            return string.Empty;
        }
    }
}
