using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Requests.Events;

public class EventSearchRequest
{
    public List<int>? TagIds { get; set; } = [];
    public int? CategoryId { get; set; }
    public int? LocationId { get; set; }
}