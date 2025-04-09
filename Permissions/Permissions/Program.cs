using System.Reflection;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Nest;
using Permissions.Data;
using Permissions.Interfaces;
using Permissions.Models;
using Permissions.Repositories;
using Permissions.Services;
using Serilog;
using Serilog.Events;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/permissions-api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configure Entity Framework
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

    // MediatR
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    // Elasticsearch
    var elasticSettings = new ConnectionSettings(new Uri(builder.Configuration["Elasticsearch:Url"]))
        .DefaultIndex("permissions");
    builder.Services.AddSingleton<IElasticClient>(new ElasticClient(elasticSettings));
    builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();

    // Kafka
    builder.Services.AddSingleton<IKafkaProducer>(provider =>
    {
        var config = new ProducerConfig
        {
            BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
            MessageTimeoutMs = 5000,
            RequestTimeoutMs = 3000
        };
        return new KafkaProducer(config,
            builder.Configuration["Kafka:Topic"],
            provider.GetRequiredService<ILogger<KafkaProducer>>());
    });

    // Repository Pattern
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

    // Logging
    builder.Host.UseSerilog();
    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    // Apply migrations and seed data
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated(); // Creates database and tables if they don't exist
            // Or use this for migrations:
            // db.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database");
        }
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}