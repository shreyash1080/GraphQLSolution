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
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text; // Add this using directive


var builder = WebApplication.CreateBuilder(args);





// Register Hot Chocolate GraphQL Server
builder.Services.AddGraphQLServer()
    .AddAuthorization() // Enable authorization
    .AddQueryType<Query>() // Adds the root Query type
    .AddMutationType<Mutation>()
    .AddType<AddProductInputType>()// Register ProductType for GraphQL
    .AddType<ProductType>()// Register AddProductType for GraphQL
    .AllowIntrospection(true) // Enable this temporarily
    .ModifyOptions(options => options.StrictValidation = false)
     .ModifyRequestOptions(o =>
     {
         o.ExecutionTimeout = TimeSpan.FromMinutes(3);
         o.IncludeExceptionDetails = true;
     });



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero // Strict expiration validation
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                return Task.CompletedTask;
            }
        };
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

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, timeout: TimeSpan.FromSeconds(5))
    .AddKafka(new ProducerConfig
    {
        BootstrapServers = "pkc-619z3.us-east1.gcp.confluent.cloud:9092",
        SecurityProtocol = SecurityProtocol.SaslSsl,
        SaslMechanism = SaslMechanism.Plain,
        SaslUsername = "656XTLT7FQD4EMQG",
        SaslPassword = "oNi81vYy9QCzLLmhpc1odNH9L9nSNZeDcXrPzv8SRdQWxJYddmOa/kUV1wHjPsZL",
        SocketTimeoutMs = 5000,
        MessageTimeoutMs = 5000,
        RequestTimeoutMs = 5000
    }, timeout: TimeSpan.FromSeconds(10));

// Add CORS policy before building the app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


// Enable CORS before other middleware
app.UseCors("AllowAll");
app.UseRouting();
app.MapHealthChecks("/health");

// Configure the GraphQL endpoint
app.MapGraphQL(); // Maps the endpoint at /graphql

// Optional: Use HTTPS redirection for production environments
app.UseHttpsRedirection();

app.Run();