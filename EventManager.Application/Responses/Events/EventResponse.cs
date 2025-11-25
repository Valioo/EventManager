using EventManager.Application.Responses.Categories;
using EventManager.Application.Responses.Location;
using EventManager.Application.Responses.Tags;
using EventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Responses.Events;

public class EventResponse
{
    public EventResponse(Event newEvent)
    {
        EventId = newEvent.Id;
        Title = newEvent.Title;
        Description = newEvent.Description;
        StartDate = newEvent.StartDate;
        EndDate = newEvent.EndDate;
        Category = new CategoryResponse(newEvent.Category);
        Location = new LocationResponse(newEvent.Location);
        if (newEvent.EventTags is not null && newEvent.EventTags.Count > 0)
        {
            Tags = [.. newEvent.EventTags.Select(et => new TagResponse(et.Tag))];
        }
        else
        {
            Tags = [];
        }
    }

    public int EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public CategoryResponse Category { get; set; }
    public LocationResponse Location { get; set; }
    public IList<TagResponse> Tags { get; set; }
}
