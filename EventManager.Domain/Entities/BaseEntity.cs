namespace EventManager.Domain.Entities;

public class BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
}