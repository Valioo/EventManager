using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("EventTags")]
public class EventTag
{
    public int EventId { get; set; }
    public Event Event { get; set; }

    public int TagId { get; set; }
    public Tag Tag { get; set; }

    public EventTag()
    {
        
    }

    public EventTag(int eventId, int tagId)
    {
        EventId = eventId;
        TagId = tagId;
    }
}
