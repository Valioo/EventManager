using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Helpers.Pagination;

public class PaginationQuery
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
