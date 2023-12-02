using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.Common.Dto.Request
{
    public class BaseFilterRequest
    {
        public string? keyword { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public IList<SortInfo>? sortInfoList { get; set; }
        public IList<FilterInfo>? filterInfoList { get; set; }

    }
}
