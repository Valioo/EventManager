using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Jobs;

public interface IEventNotificationJob
{
    Task RunAsync();
}