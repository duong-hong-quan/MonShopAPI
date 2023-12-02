using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.Common.Dto.Response
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
