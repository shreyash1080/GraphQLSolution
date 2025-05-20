using Microsoft.Data.SqlClient; // Required for SQL operations (modern replacement for System.Data.SqlClient)
using System.Collections.Generic; // For using generic collections like List<T>
using System.Threading.Tasks; // For asynchronous programming
using Core.Entities;
using Core.Interfaces;
using Application.Models;
using System.Data;
using Microsoft.Extensions.Logging;
using Common.Response;
using Dapper;
using System; // Reference to the Product class defined in the Core.Entities namespace

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repository class for handling database operations related to Products.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;


        /// <summary>
        /// Constructor to initialize the repository with a database connection string.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString; // Connection string provided by the application.
        }

        /// <summary>
        /// Asynchronously retrieves a list of products from the database.
        /// </summary>
        /// <returns>List of Product objects.</returns>
        public async Task<List<Product>> GetProductsAsync(int userID)
        {
            var products = new List<Product>(); // Collection to store retrieved products.

            // Using statement ensures that the connection is properly disposed of after use.
            using (var connection = new SqlConnection(_connectionString))
            {
                // OpenAsync is used instead of Open to prevent blocking the thread.
                // It allows the application to handle other tasks while waiting for the database connection to open.
                await connection.OpenAsync();

                // Define a SQL command to execute. The SQL query fetches Id, Name, and Price columns from the Products table.
                var command = new SqlCommand("SELECT Id,Name,CreatedAt,Description,Stock,IsAvailable, Category,Price,SkuId,Supplier,Discount FROM Products WHERE UserID = @UserId", connection);

                command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar) { Value = userID });

                // ExecuteReaderAsync runs the query and provides a reader for streaming rows from the database.
                using (var reader = await command.ExecuteReaderAsync())
                {
                    // Read each row asynchronously until no rows remain.
                    while (await reader.ReadAsync())
                    {
                        // Add each row's data to the products list by mapping the columns to a Product object.
                        products.Add(new Product
                        {
                            Id = reader.GetInt32("Id"), // Get the value of the first column (Id) as an integer.
                            Name = reader.GetString("Name"), // Get the value of the second column (Name) as a string.
                            Price = reader.IsDBNull("Price") ? 0 : reader.GetDecimal("Price"), // Handle potential null values
                            CreatedAt = reader.IsDBNull("CreatedAt") ? null : reader.GetDateTime("CreatedAt"), // Handle potential null values
                            Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"), // Handle potential null values
                            Stock = reader.IsDBNull("Stock") ? 0 : reader.GetInt32("Stock"), // Handle potential null values
                            IsAvailable = reader.IsDBNull("IsAvailable") ? false : reader.GetBoolean("IsAvailable"), // Handle potential null values
                            Category = reader.IsDBNull("Category") ? null : reader.GetString("Category"), // Handle potential null values
                            SkuID = reader.IsDBNull("SkuId") ? null : reader.GetString("SkuId"), // Handle potential null values
                            Supplier = reader.IsDBNull("Supplier") ? null : reader.GetString("Supplier"), // Handle potential null values
                            Discount = reader.IsDBNull("Discount") ? 0 : reader.GetDecimal("Discount"), // Handle potential null values
                        });
                    }
                }
            }

            // Return the list of products to the caller.
            return products;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            // Create a SQL connection using the provided connection string
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(); // Open the connection asynchronously

            // Define the SQL query with OUTPUT to return the inserted row
            using var command = new SqlCommand(
        "INSERT INTO Products (Name, Price, CreatedAt, Description, Stock, IsAvailable, Category,SkuId,Supplier,Discount,UserID) " +
        "OUTPUT INSERTED.Id, INSERTED.Name, INSERTED.Price, INSERTED.CreatedAt, INSERTED.Description, " +
        "INSERTED.Stock, INSERTED.IsAvailable, INSERTED.Category, INSERTED.SkuId, " +
        "INSERTED.Supplier, INSERTED.Discount, INSERTED.UserID " +
        "VALUES (@Name, @Price, GETDATE(), @Description, @Stock, @IsAvailable, @Category,@SkuId,@Supplier,@Discount,@UserID)",
        connection);

            // Add parameters to the command to prevent SQL injection
            //SqlDbType It's an enum representing SQL Server data types in C#.
            // Add parameters to the command to prevent SQL injection
            command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = product.Name });
            command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Value = product.Price });
            command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar)
            {
                Value = (object?)product.Description ?? DBNull.Value
            });
            command.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = product.Stock });
            command.Parameters.Add(new SqlParameter("@IsAvailable", SqlDbType.Bit) { Value = product.IsAvailable });
            command.Parameters.Add(new SqlParameter("@Category", SqlDbType.NVarChar) { Value = product.Category });
            command.Parameters.Add(new SqlParameter("@SkuId", SqlDbType.NVarChar) { Value = product.SkuID });
            command.Parameters.Add(new SqlParameter("@Supplier", SqlDbType.NVarChar) { Value = product.Supplier });
            command.Parameters.Add(new SqlParameter("@Discount", SqlDbType.Decimal) { Value = product.Discount });
            command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar) { Value = product.UserId });




            // Execute the command and read the output values
            //Reads multiple columns returned from the OUTPUT clause
            //Allows us to fetch Id, Name, and Price after insertion
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var insertedProduct = new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                    IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                    Category = reader.GetString(reader.GetOrdinal("Category")),
                    SkuID = reader.GetString(reader.GetOrdinal("SkuId")),
                    Supplier = reader.GetString(reader.GetOrdinal("Supplier")),
                    Discount = reader.GetDecimal(reader.GetOrdinal("Discount")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserID"))
                };;

                return insertedProduct;
            }
            return null; // If no row was inserted, return null
        }

        public async Task<Product> DeleteProductAsync(int productId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(); // Open the connection asynchronously

            // Define the SQL query with OUTPUT to return deleted details
            using var command = new SqlCommand(
                @"DELETE FROM Products 
                 OUTPUT DELETED.Id, DELETED.Name 
                WHERE Id = @Id",
                connection);

            // Add parameter to prevent SQL injection
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = productId });

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                // Create and populate the deleted product
                var deletedProduct = new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                };

                // Log the deleted product
                Console.WriteLine($"✅ Product Deleted - ID: {deletedProduct.Id}, Name: {deletedProduct.Name}");

                return deletedProduct; // Return the deleted product for confirmation
            }

            // Log and return null if no product was deleted
            Console.WriteLine("❌ No Product Found to Delete.");
            return null;
        }
        public async Task<Product> UpdateProductRepositoryAsync(Product m)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"
UPDATE Products
SET
    Name         = COALESCE(@Name,         Name),
    Price        = COALESCE(@Price,        Price),
    Description  = COALESCE(@Description,  Description),
    Stock        = COALESCE(@Stock,        Stock),
    IsAvailable  = COALESCE(@IsAvailable,  IsAvailable),
    Category     = COALESCE(@Category,     Category),
    SkuId        = COALESCE(@SkuId,        SkuId),
    Supplier     = COALESCE(@Supplier,     Supplier),
    Discount     = COALESCE(@Discount,     Discount)
OUTPUT INSERTED.*
WHERE Id = @Id";

            // Dapper will bind null for any missing field and COALESCE keeps old value.
            var updated = await conn.QuerySingleOrDefaultAsync<Product>(
                sql,
                new
                {
                    m.Id,
                    m.Name,
                    m.Price,
                    m.Description,
                    m.Stock,
                    m.IsAvailable,
                    m.Category,
                    m.SkuID,
                    m.Supplier,
                    m.Discount,
                });
            return updated;
        }




    }
}



//What is using var?
//using var reader = await command.ExecuteReaderAsync();
//✅ The using keyword ensures that resources are properly disposed of after use.
//✅ var is used to automatically infer the type (SqlDataReader in this case).
//Why Do We Need to Dispose of SqlDataReader?
//Problem Without using
//SqlDataReader opens a database connection and keeps it open until manually closed. If we forget to close/dispose of it, we could: ❌ Cause memory leaks
//❌ Keep connections open, leading to connection pool exhaustion
//❌ Risk locking database tables, affecting performance

//Solution with using
//The using keyword ensures that SqlDataReader is disposed of automatically when it goes out of scope.

//✅ No need to manually close reader.Close()
//✅ Prevents memory leaks & database connection issues
//✅ Code is cleaner and safer