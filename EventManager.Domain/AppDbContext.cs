using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventManager.Domain;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Location> Locations { get; set; }

    public DbSet<Tag> Tags { get; set; }
    public DbSet<EventTag> EventTags { get; set; }

    public DbSet<EventSubscription> EventSubscriptions { get; set; }

    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<Notification> Notifications { get; set; }
    public DbSet<EventNotification> EventNotifications { get; set; }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ProcessAuditChanges();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ProcessAuditChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ProcessAuditChanges()
    {
        var utcNow = DateTimeOffset.UtcNow;

        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = utcNow;
                }

                baseEntity.ModifiedAt = utcNow;
            }
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ----------------------------
        // Composite keys
        // ----------------------------

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<EventTag>()
            .HasKey(et => new { et.EventId, et.TagId });

        modelBuilder.Entity<EventSubscription>()
            .HasKey(ep => new { ep.EventId, ep.UserId });

        modelBuilder.Entity<EventNotification>()
            .HasKey(ep => new { ep.EventId, ep.NotificationId });

        // ----------------------------
        // Indexes / Uniqueness
        // ----------------------------

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        modelBuilder.Entity<TicketType>()
            .Property(x => x.Price)
            .HasPrecision(18, 4);

        modelBuilder.Entity<User>()
            .HasQueryFilter(u => !u.IsDeleted);

        modelBuilder.Entity<Event>()
            .HasQueryFilter(u => !u.IsDeleted);

        // ----------------------------
        // Relationships
        // ----------------------------

        // Notification <-> EventNotifications (M:N)
        modelBuilder.Entity<EventNotification>()
            .HasOne(en => en.Event)
            .WithMany(en => en.EventNotifications)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventNotification>()
            .HasOne(en => en.Notification)
            .WithMany(en => en.EventNotifications)
            .HasForeignKey(x => x.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);

        // User <-> UserRole (M:N)
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event -> Category (M:1)
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event -> Location (M:1)
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Location)
            .WithMany(l => l.Events)
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event <-> EventTag (M:N)
        modelBuilder.Entity<EventTag>()
            .HasOne(et => et.Event)
            .WithMany(e => e.EventTags)
            .HasForeignKey(et => et.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventTag>()
            .HasOne(et => et.Tag)
            .WithMany(t => t.EventTags)
            .HasForeignKey(et => et.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        // Event <-> EventSubscription (M:N)
        modelBuilder.Entity<EventSubscription>()
            .HasOne(ep => ep.Event)
            .WithMany(e => e.EventSubscribers)
            .HasForeignKey(ep => ep.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventSubscription>()
            .HasOne(ep => ep.User)
            .WithMany(u => u.EventSubscribers)
            .HasForeignKey(ep => ep.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event -> TicketType (1:N)
        modelBuilder.Entity<TicketType>()
            .HasOne(tt => tt.Event)
            .WithMany(e => e.TicketTypes)
            .HasForeignKey(tt => tt.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ticket -> TicketType (N:1)
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.TicketType)
            .WithMany(tt => tt.Tickets)
            .HasForeignKey(t => t.TicketTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ticket -> User (N:1)
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}