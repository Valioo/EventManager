using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Categories")]
public class Category : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    // Navigation
    public ICollection<Event> Events { get; set; }

    public Category()
    {
        
    }

    public Category(string name)
    {
        Name = name;
    }
}
