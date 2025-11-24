using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Roles")]
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation
    public ICollection<UserRole> UserRoles { get; set; }
}
