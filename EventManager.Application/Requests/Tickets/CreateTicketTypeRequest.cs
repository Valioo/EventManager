using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Requests.Tickets;

public class CreateTicketTypeRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Capacity { get; set; }
    public int EventId { get; set; }
}