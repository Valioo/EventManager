using EventManager.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Jobs;

public class EventNotificationJob : IEventNotificationJob
{
    private readonly AppDbContext _appDbContext;

    public EventNotificationJob(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task RunAsync()
    {
        var today = DateTime.UtcNow.Date;

        // Get all events with notifications where StartDate - DaysPriorStart == today
        var eventMatches = await _appDbContext.Events
            .Where(e => !e.IsDeleted)
            .SelectMany(e => e.EventNotifications, (e, en) => new { Event = e, en.Notification })
            .Where(x => x.Event.StartDate.Date.AddDays(-x.Notification.DaysPriorStart) == today.Date)
            .ToListAsync();

        if (eventMatches.Count == 0)
            return;

        foreach (var match in eventMatches)
        {
            var evt = match.Event;

            var subscribers = await _appDbContext.EventSubscriptions
                .Where(s => s.EventId == evt.Id)
                .Select(s => s.User)
                .ToListAsync();

            foreach (var user in subscribers)
            {
                // Send some emails
            }
        }
    }
}