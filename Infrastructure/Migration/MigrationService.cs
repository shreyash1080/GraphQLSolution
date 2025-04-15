using FluentMigrator.Runner; // Provides migration runner services
using Microsoft.Extensions.DependencyInjection; // Required for Dependency Injection
using System;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Migrations
{
    public class MigrationService
    {
        /// <summary>
        /// Runs all pending database migrations using FluentMigrator.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        public static void RunMigrations(string connectionString)
        {
            // ✅ STEP 1: Configure the DI (Dependency Injection) container
            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole())
                .AddFluentMigratorCore() // Registers FluentMigrator services
                .ConfigureRunner(rb => rb
                   .AddSqlServer() // Specifies SQL Server as the database type
                    .WithGlobalConnectionString(connectionString) // Sets the connection string
                    .ScanIn(typeof(CreateProductsTable).Assembly).For.Migrations()) // Scans for CreateProductsTable and related migrations
                                                                                  
                .BuildServiceProvider(false); // Builds the service provider

            // ✅ STEP 2: Create a DI scope for running the migration
            //Ensures that the migration runner only runs within this scope and is properly disposed of afterward.
            using var scope = serviceProvider.CreateScope();

            // ✅ STEP 3: Get the migration runner from DI
            //IMigrationRunner is FluentMigrator's engine for executing migrations.
            //It is retrieved from the DI container
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            //This runner will now be responsible for running all pending migrations.


            Console.WriteLine("🚀 Running Migrations...");
            // ✅ STEP 4: Apply all pending migrations
            runner.MigrateUp(); // Runs all migrations found in the scanned assembly
                                //Finds and executes all migration classes ([Migration(20250401
                                //Ensures the database schema is up to date.
            Console.WriteLine("✅ Migrations Completed!");

        }
    }
}

//The MigrationService class is responsible for running FluentMigrator migrations in the application.
//It sets up FluentMigrator, finds all migration classes, and applies them to the database.

//Why do we need this?

//AddFluentMigratorCore() → Registers FluentMigrator services in DI.

//AddSqlServer() → Specifies that migrations should be applied to a SQL Server database.

//WithGlobalConnectionString(connectionString) → Passes the database connection string.

//ScanIn(typeof(CreateProductsTable).Assembly).For.Migrations() → Scans the assembly for migration classes.

//This ensures FluentMigrator automatically discovers and runs all migration scripts.
