using API.GraphQl;
using API.GraphQl.Types.InputType;
using API.GraphQL;
using Application.Services;
using Core.Interfaces;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register Hot Chocolate GraphQL Server
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>() // Adds the root Query type
    .AddMutationType<Mutation>()
    .AddType<ProductType>();// Register ProductType for GraphQL

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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


var app = builder.Build();

// Configure the GraphQL endpoint
app.MapGraphQL(); // Maps the endpoint at /graphql

// Optional: Use HTTPS redirection for production environments
app.UseHttpsRedirection();

app.Run();
