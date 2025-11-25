using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Tags")]
public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; }

    // Navigation
    public ICollection<EventTag> EventTags { get; set; }

    public Tag()
    {
        
    }

    public Tag(string name)
    {
        Name = name;
    }
}
