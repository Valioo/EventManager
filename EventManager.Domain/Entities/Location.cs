using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Locations")]
public class Location
{
    public int Id { get; set; }

    public string Address { get; set; }
    public string City { get; set; }
    public string VenueName { get; set; }

    // Navigation
    public ICollection<Event> Events { get; set; }
}
