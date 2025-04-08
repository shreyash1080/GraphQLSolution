using API.GraphQl;
using API.GraphQl.Types.InputType;
using API.GraphQL;
using Application.Services;
using Core.Interfaces;
using Infrastructure.Migrations;
using Infrastructure.Repositories;
using KafkaProducer.Configuration;
using KafkaProducer.Publisher;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Register Hot Chocolate GraphQL Server
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>() // Adds the root Query type
    .AddMutationType<Mutation>() // Adds the root Mutation type
    .AddType<AddProductInputType>() // Registers AddProductInputType for GraphQL input definitions
    .AddType<ProductType>(); // Registers ProductType for GraphQL output definitions

// Retrieve the database connection string from configuration
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ Run Migrations Before Registering Repositories
// Ensures that database schema is up to date by applying any pending migrations
MigrationService.RunMigrations(connectionString);

// Register the IProductRepository interface with a singleton lifetime.
// - `AddSingleton` ensures only one instance of ProductRepository is created and used throughout the application's lifetime.
// - `IProductRepository` is the abstraction, and `ProductRepository` is the implementation being registered here.
builder.Services.AddSingleton<IProductRepository>(provider =>
    // A lambda is used to provide the dependency (ProductRepository instance).
    // The connection string is passed to the ProductRepository constructor.
    new ProductRepository(connectionString));

// Register the ProductService class with a scoped lifetime.
// - `AddScoped` ensures a new instance of ProductService is created per HTTP request.
// - This is ideal for services like ProductService that rely on request-specific data or processing.
builder.Services.AddScoped<ProductService>();

// Configure Kafka settings by binding the "Kafka" section from appsettings.json
builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("Kafka"));

// Register KafkaConfig as a singleton service for DI
builder.Services.AddSingleton<KafkaConfig>(sp =>
    sp.GetRequiredService<IOptions<KafkaConfig>>().Value);

// Register Kafka Producer dependencies
builder.Services.AddSingleton<ITopicPublisher, TopicPublisher>();

builder.Environment.EnvironmentName = "Development";


var app = builder.Build();

// Map the GraphQL endpoint
// - The endpoint is accessible at /graphql and handles all GraphQL API requests.
app.MapGraphQL();

// Use GraphQL Playground for UI testing and query exploration
// - Accessible at /graphql-playground
// - Provides an interactive interface for testing GraphQL queries and mutations
app.UseGraphQLPlayground("/graphql-playground", new GraphQL.Server.Ui.Playground.PlaygroundOptions
{
    GraphQLEndPoint = "/graphql" // Specifies the GraphQL API endpoint
});

// Optional: Use HTTPS redirection for production environments
// - Redirects all HTTP requests to HTTPS for secure communication
app.UseHttpsRedirection();

// Start the application
app.Run();
