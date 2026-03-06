using EventManager.Domain;
using EventManager.Domain.Seed;
using Microsoft.EntityFrameworkCore;
using EventManager.Application.Configuration;
using EventManager.API.Bootstrapper;
using Hangfire;
using EventManager.Application.Jobs;
using Amazon.SimpleNotificationService;
using Amazon;

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

var awsConfig = builder.Configuration.GetSection("AWS");
builder.Services.AddSingleton<IAmazonSimpleNotificationService>(sp =>
{
    return new AmazonSimpleNotificationServiceClient(
        awsConfig["AccessKey"],
        awsConfig["SecretKey"],
        new AmazonSimpleNotificationServiceConfig
        {
            ServiceURL = awsConfig["ServiceURL"],
            AuthenticationRegion = awsConfig["Region"],
            UseHttp = true,
            //RegionEndpoint = RegionEndpoint.GetBySystemName(awsConfig["Region"])
        });
});

builder.Services.AddHangfireServer();

builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["http://localhost:4200"];
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHangfireDashboard();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(db, builder.Configuration.GetSection("Auth"));
}

RecurringJob.AddOrUpdate<IEventNotificationJob>(
    "daily-event-notification-check",
    job => job.RunAsync(),
    Cron.Daily
);

app.Run();
