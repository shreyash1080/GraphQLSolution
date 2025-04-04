using FluentMigrator; // Provides core migration functionalities
using FluentMigrator.Builders.Create.Table; // Allows fluent API for table creation
using System; // Required for Action<T> delegate

namespace Infrastructure.Migrations
{
    /// <summary>
    /// Provides extension methods for FluentMigrator to handle table creation if it does not exist.
    /// </summary>
    public static class FluentMigratorExtensions
    {
        /// <summary>
        /// Checks if a table exists in the specified schema and creates it if it does not exist.
        /// </summary>
        /// <param name="migration">The migration context (inherits from MigrationBase)</param>
        /// <param name="tableName">Name of the table to create</param>
        /// <param name="constructTableFunc">Delegate that defines the table structure</param>
        /// <param name="schemaName">Schema name (default is "dbo")</param>
        public static void CreateTableIfNotExists(
            this MigrationBase migration,
            string tableName,
            Action<ICreateTableWithColumnSyntax> constructTableFunc,
            string schemaName = "dbo")
        {
            // Step 1: Check if the table already exists in the schema
            if (migration.Schema.Schema(schemaName).Table(tableName).Exists())
            {
                return; // If it exists, do nothing (skip table creation)
            }

            // Step 2: Define the new table
            var table = migration.Create.Table(tableName).InSchema(schemaName);

            // Step 3: Apply the provided structure (columns, constraints, etc.)
            constructTableFunc(table);
        }
    }
}


//schema is a namespace for database objects.
//Example: dbo.Products means the Products table belongs to the dbo schema.