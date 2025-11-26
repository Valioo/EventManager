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

    // DbSets
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get the connection string from appsettings.json
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        // Configure the DbContext to use SQL Server with the connection string
        optionsBuilder.UseSqlServer(connectionString);
    }

    //public DbSet<Image> Images { get; set; }       // optional if you add Images entity later
    //public DbSet<Message> Messages { get; set; }   // optional if you add Messages entity later

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ----------------------------
        // Table names (optional)
        // ----------------------------
        //modelBuilder.Entity<User>().ToTable("Users");
        //modelBuilder.Entity<Role>().ToTable("Roles");
        //modelBuilder.Entity<UserRole>().ToTable("UserRoles");

        //modelBuilder.Entity<Event>().ToTable("Events");
        //modelBuilder.Entity<Category>().ToTable("Categories");
        //modelBuilder.Entity<Location>().ToTable("Locations");

        //modelBuilder.Entity<Tag>().ToTable("Tags");
        //modelBuilder.Entity<EventTag>().ToTable("EventTags");

        //modelBuilder.Entity<EventParticipant>().ToTable("EventParticipants");

        //modelBuilder.Entity<TicketType>().ToTable("TicketTypes");
        //modelBuilder.Entity<Ticket>().ToTable("Tickets");

        // Images & Messages tables are optional - create only if you included the classes
        // modelBuilder.Entity<Image>().ToTable("Images");
        // modelBuilder.Entity<Message>().ToTable("Messages");

        // ----------------------------
        // Keys & composite keys
        // ----------------------------

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<EventTag>()
            .HasKey(et => new { et.EventId, et.TagId });

        modelBuilder.Entity<EventSubscription>()
            .HasKey(ep => new { ep.EventId, ep.UserId });

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

        // If you add SerialNumber to Ticket later, create unique index here:
        // modelBuilder.Entity<Ticket>().HasIndex(t => t.SerialNumber).IsUnique();

        // ----------------------------
        // Relationships
        // ----------------------------

        // User <-> UserRole (M:N)
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict); // deleting user removes its roles links

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event -> Category (many events to one category). Category can be optional.
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event -> Location (many events to one location). Location required.
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
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventTag>()
            .HasOne(et => et.Tag)
            .WithMany(t => t.EventTags)
            .HasForeignKey(et => et.TagId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event <-> EventParticipant (M:N)
        modelBuilder.Entity<EventSubscription>()
            .HasOne(ep => ep.Event)
            .WithMany(e => e.EventParticipants)
            .HasForeignKey(ep => ep.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventSubscription>()
            .HasOne(ep => ep.User)
            .WithMany(u => u.EventParticipants)
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

        // If you added Image and Message entities, wire them up here (examples commented)
        // modelBuilder.Entity<Image>()
        //     .HasOne(i => i.Event)
        //     .WithMany(e => e.Images)
        //     .HasForeignKey(i => i.EventId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<Message>()
        //     .HasOne(m => m.FromUser)
        //     .WithMany() // optionally create navigation on User
        //     .HasForeignKey(m => m.FromUserId)
        //     .OnDelete(DeleteBehavior.Restrict);
    }
}