using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.BackEnd.Common.Dto.Response;

namespace MonShop.BackEnd.Common.Dto.Request
{
    public class AppActionResult
    {
        public Result Result { get; set; } = new();

        public bool IsSuccess { get; set; } = true;
        public List<string?> Messages { get; set; } = new List<string?>();

    }
}
