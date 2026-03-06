using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using EventManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EventManager.Application.Jobs;

public class EventNotificationJob : IEventNotificationJob
{
    private readonly AppDbContext _appDbContext;
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly IConfiguration _configuration;

    public EventNotificationJob(AppDbContext appDbContext, IAmazonSimpleNotificationService snsClient, IConfiguration configuration)
    {
        _appDbContext = appDbContext;
        _snsClient = snsClient;
        _configuration = configuration;
    }
    public async Task RunAsync()
    {
        var today = DateTime.UtcNow.Date;

        var eventMatches = await _appDbContext.Events
            .SelectMany(e => e.EventNotifications, (e, en) => new { Event = e, en.Notification })
            .Where(x => x.Event.StartDate.Date.AddDays(-x.Notification.DaysPriorStart) == today.Date)
            .ToListAsync();

        if (eventMatches.Count == 0)
            return;

        var topicArn = _configuration["AWS:SNSTopicArn"];

        foreach (var match in eventMatches)
        {
            var evt = match.Event;

            var emails = await _appDbContext.EventSubscriptions
                .Where(s => s.EventId == evt.Id)
                .Select(s => s.User.Email)
                .ToListAsync();

            var payload = new
            {
                EventId = evt.Id,
                EventName = evt.Title,
                Emails = emails
            };

            var publishRequest = new PublishRequest
            {
                TopicArn = topicArn,
                Message = JsonConvert.SerializeObject(payload),
                Subject = $"Notification for Event: {evt.Title}"
            };

            try
            {
                await _snsClient.PublishAsync(publishRequest);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}