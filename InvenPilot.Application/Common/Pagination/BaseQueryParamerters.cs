using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Common.Pagination
{
    public class BaseQueryParamerters
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool Desc { get; set; }
    }
}
