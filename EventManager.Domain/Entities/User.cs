using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Users")]
public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Navigation
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<EventSubscription> EventParticipants { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}
