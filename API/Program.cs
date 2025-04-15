using API.GraphQl;
using API.GraphQl.Types.InputType;
using API.GraphQL;
using Application.Services;
using Application.AutoMapping;
using Core.Interfaces;
using Infrastructure.Migrations;
using Infrastructure.Repositories;
using KafkaProducer.Configuration;
using KafkaProducer.Publisher;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Diagnostics.HealthChecks; // Add this using directive


var builder = WebApplication.CreateBuilder(args);





// Register Hot Chocolate GraphQL Server
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>() // Adds the root Query type
    .AddMutationType<Mutation>()
    .AddType<AddProductInputType>()// Register ProductType for GraphQL
    .AddType<ProductType>()// Register AddProductType for GraphQL
    .ModifyRequestOptions(opt =>
    {
        opt.ExecutionTimeout = TimeSpan.FromMinutes(3); // Increase timeout to 3 minutes
        opt.IncludeExceptionDetails = true; // Show detailed errors
    });

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ Run Migrations Before Registering Repositories
MigrationService.RunMigrations(connectionString);

// Register the IProductRepository interface with a singleton lifetime.
// - `AddSingleton` ensures only one instance of ProductRepository is created and used throughout the application's lifetime.
// - `IProductRepository` is the abstraction, and `ProductRepository` is the implementation being registered here.
builder.Services.AddSingleton<IProductRepository>(provider =>
    // A lambda is used to provide the dependency (ProductRepository instance).
    // The connection string is passed to the ProductRepository constructor.
    new ProductRepository(connectionString));

builder.Services.AddSingleton<IUserRepository>(provider => (IUserRepository)new UserRepository(connectionString));


// Register the ProductService class with a scoped lifetime.
// - `AddScoped` ensures a new instance of ProductService is created per HTTP request.
// - This is ideal for services like ProductService that rely on request-specific data or processing.
builder.Services.AddScoped<ProductService>();

builder.Services.AddScoped<UserService>();




builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<KafkaConfig>(sp =>
    sp.GetRequiredService<IOptions<KafkaConfig>>().Value);

// Register Kafka Producer Dependencies
builder.Services.AddSingleton<ITopicPublisher, TopicPublisher>();


// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));




var app = builder.Build();

// Add health checks
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, timeout: TimeSpan.FromSeconds(5))
    .AddKafka(new ProducerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"]
    });

app.MapHealthChecks("/health");


// Configure the GraphQL endpoint
app.MapGraphQL(); // Maps the endpoint at /graphql

// Optional: Use HTTPS redirection for production environments
app.UseHttpsRedirection();

app.Run();