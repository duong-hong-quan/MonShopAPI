using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.Common.Dto.Request
{
    public class SignUpRequestDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool? Gender { get; set; }
        public string PhoneNumber { get; set; }

        public List<string> RoleName { get; set; }

    }
}
