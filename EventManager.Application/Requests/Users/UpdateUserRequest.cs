using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Requests.Users;

public class UpdateUserRequest
{
    public int UserId { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
}
