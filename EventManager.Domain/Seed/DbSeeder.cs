using EventManager.Domain.Entities;
using EventManager.Domain.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventManager.Domain.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IConfigurationSection authSection)
    {
        await AddRoles(db);

        var user = await db.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.FullName == "Admin");
        if (user is not null && user.IsDeleted)
        {
            user.IsDeleted = false;
            await db.SaveChangesAsync();
        }
        else if(user is null)
        {
            var admin = new User
            {
                FullName = "Admin",
                Email = "admin@gmail.com",
                PasswordHash = ""
            };

            admin.PasswordHash = PasswordHashing.HashPassword("admin");
            db.Users.Add(admin);
            await db.SaveChangesAsync();

            var adminRole = await db.Roles.FirstAsync(r => r.Name == "Administrator");

            db.UserRoles.Add(new UserRole
            {
                UserId = admin.Id,
                RoleId = adminRole.Id
            });

            await db.SaveChangesAsync();
        }
    }

    private static async Task AddRoles(AppDbContext db)
    {
        var rolesToAdd = new List<Role>();

        if (!await db.Roles.AnyAsync(a => a.Name == "Administrator"))
        {
            var adminRole = new Role { Name = "Administrator" };
            rolesToAdd.Add(adminRole);
        }

        if (!await db.Roles.AnyAsync(a => a.Name == "Organizer"))
        {
            var organizerRole = new Role { Name = "Organizer" };
            rolesToAdd.Add(organizerRole);
        }

        if (rolesToAdd.Count != 0)
        {
            await db.AddRangeAsync(rolesToAdd);
            await db.SaveChangesAsync();
        }
    }
}
