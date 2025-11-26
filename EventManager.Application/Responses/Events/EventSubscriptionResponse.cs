using EventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Responses.Events;

public class EventSubscriptionResponse
{
    public string EventTitle { get; set; }
    public int EventId { get; set; }

    public EventSubscriptionResponse(EventSubscription eventSubscription)
    {
        EventTitle = eventSubscription.Event.Title;
        EventId = eventSubscription.Event.Id;
    }
}