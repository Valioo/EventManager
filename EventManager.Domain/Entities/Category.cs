using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Categories")]
public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    // Navigation
    public ICollection<Event> Events { get; set; }
}
