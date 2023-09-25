using MonShopLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.Repository.IRepository
{
    public interface IJwtGenerator
    {
        Task<string> GenerateAccessToken(LoginRequest loginRequest);
        string GenerateRefreshToken();
        Task<TokenModel> GetNewToken(string refreshToken, string accountId);


    }
}
