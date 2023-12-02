using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.Common.Dto.Response;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface IJwtService
    {
        string GenerateRefreshToken();
        Task<string> GenerateAccessToken(LoginRequestDto loginRequest);
        Task<TokenDto> GetNewToken(string refreshToken, string accountId);


    }
}
