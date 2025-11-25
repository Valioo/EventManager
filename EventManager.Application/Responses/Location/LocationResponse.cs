namespace EventManager.Application.Responses.Location;

public class LocationResponse
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string VenueName { get; set; }

    public LocationResponse()
    {
    }

    public LocationResponse(Domain.Entities.Location location)
    {
        Id = location.Id;
        Address = location.Address;
        City = location.City;
        VenueName = location.VenueName;
    }
}
