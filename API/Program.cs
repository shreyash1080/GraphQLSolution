using API.GraphQl;
using API.GraphQl.Types.InputType;
using API.GraphQL;
using Application.Services;
using Core.Interfaces;
using FluentMigrator.Runner;
using Infrastructure.Migrations;
using Infrastructure.Repositories;
using KafkaProducer.Configuration;
using KafkaProducer.Publisher;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Kestrel FIRST for Azure port binding
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080); // Explicit Azure port configuration
});

// 2. Add services in proper order
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<AddProductInputType>()
    .AddType<ProductType>();

// 3. Configure logging early
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// 4. Database Configuration with Error Handling
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string is not configured");
}

//// 5. Configure FluentMigrator with DI
//builder.Services.AddFluentMigratorCore()
//    .ConfigureRunner(rb => rb
//        .AddPostgres()
//        .WithGlobalConnectionString(connectionString)
//        .ScanIn(typeof(AddUsersTable).Assembly).For.Migrations())
//    .AddLogging(lb => lb.AddFluentMigratorConsole());

// 6. Service Registrations
builder.Services.AddSingleton<IProductRepository>(new ProductRepository(connectionString));
builder.Services.AddScoped<ProductService>();
builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<KafkaConfig>(sp =>
    sp.GetRequiredService<IOptions<KafkaConfig>>().Value);
builder.Services.AddSingleton<ITopicPublisher, TopicPublisher>();

var app = builder.Build();

//// 7. Run Migrations with proper error handling
//using (var scope = app.Services.CreateScope())
//{
//    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
//    try
//    {
//        runner.MigrateUp();
//    }
//    catch (Exception ex)
//    {
//        app.Logger.LogCritical(ex, "Database migration failed");
//        throw; // Ensure failure is visible in Azure logs
//    }
//}

// 8. Configure endpoints
app.MapGraphQL();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL("/graphql");
});

// 9. Azure-specific configuration
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Logger.LogInformation("Application starting on port 8080");
app.Run();