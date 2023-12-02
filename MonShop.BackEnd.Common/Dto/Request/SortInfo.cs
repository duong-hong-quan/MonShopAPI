using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.Common.Dto.Request
{
    public class SortInfo
    {
        public string fieldName { get; set; }
        public bool ascending { get; set; } = true;
    }
}
