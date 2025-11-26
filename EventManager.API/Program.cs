using EventManager.Domain;
using EventManager.Domain.Seed;
using Microsoft.EntityFrameworkCore;
using EventManager.Application.Configuration;
using EventManager.API.Bootstrapper;
using Hangfire;
using EventManager.Application.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection")!);
// Add services to the container.
builder.Services
    .BootstrapApplication()
    .AddJwt(builder.Configuration.GetSection("Jwt"));

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHangfireDashboard();

RecurringJob.AddOrUpdate<IEventNotificationJob>(
    "daily-event-notification-check",
    job => job.RunAsync(),
    Cron.Daily
);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(db, builder.Configuration.GetSection("Auth"));
}

app.Run();
