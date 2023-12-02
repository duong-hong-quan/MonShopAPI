using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.Common.Dto.Request
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
        public string RecoveryCode { get; set; }
        public string NewPassword { get; set; }
    }
}
