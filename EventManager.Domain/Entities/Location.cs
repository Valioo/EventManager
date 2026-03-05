using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Locations")]
public class Location : BaseEntity
{
    public int Id { get; set; }

    public string Address { get; set; }
    public string City { get; set; }
    public string VenueName { get; set; }

    // Navigation
    public ICollection<Event> Events { get; set; }

    public Location()
    {
        
    }

    public Location(string address, string city, string venueName)
    {
        Address = address;
        City = city;
        VenueName = venueName;
    }
}
