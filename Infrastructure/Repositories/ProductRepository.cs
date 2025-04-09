using Microsoft.Data.SqlClient; // Required for SQL operations (modern replacement for System.Data.SqlClient)
using System.Collections.Generic; // For using generic collections like List<T>
using System.Threading.Tasks; // For asynchronous programming
using Core.Entities;
using Core.Interfaces;
using Application.Models;
using System.Data;

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
        public async Task<List<Product>> GetProductsAsync()
        {
            var products = new List<Product>(); // Collection to store retrieved products.

            // Using statement ensures that the connection is properly disposed of after use.
            using (var connection = new SqlConnection(_connectionString))
            {
                // OpenAsync is used instead of Open to prevent blocking the thread.
                // It allows the application to handle other tasks while waiting for the database connection to open.
                await connection.OpenAsync();

                // Define a SQL command to execute. The SQL query fetches Id, Name, and Price columns from the Products table.
                var command = new SqlCommand("SELECT Id, Name, Price FROM Products", connection);

                // ExecuteReaderAsync runs the query and provides a reader for streaming rows from the database.
                using (var reader = await command.ExecuteReaderAsync())
                {
                    // Read each row asynchronously until no rows remain.
                    while (await reader.ReadAsync())
                    {
                        // Add each row's data to the products list by mapping the columns to a Product object.
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(0), // Get the value of the first column (Id) as an integer.
                            Name = reader.GetString(1), // Get the value of the second column (Name) as a string.
                            Price = reader.GetDecimal(2) // Get the value of the third column (Price) as a decimal.
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
                "INSERT INTO Products (Name, Price) " +
                "OUTPUT INSERTED.Id, INSERTED.Name, INSERTED.Price " +//OUTPUT INSERTED allows us to get the newly created row directly without needing another SELECT query
                "VALUES (@Name, @Price)",
                connection);

            // Add parameters to the command to prevent SQL injection
            //SqlDbType It's an enum representing SQL Server data types in C#.
            command.Parameters.Add(new SqlParameter("@Name", System.Data.SqlDbType.NVarChar) { Value = product.Name });
            command.Parameters.Add(new SqlParameter("@Price", System.Data.SqlDbType.Decimal) { Value = product.Price });

            // Execute the command and read the output values
            //Reads multiple columns returned from the OUTPUT clause
            //Allows us to fetch Id, Name, and Price after insertion
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var insertedProduct = new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                };

                // 🔹 Log the inserted product
                Console.WriteLine($"✅ Product Inserted - ID: {insertedProduct.Id}, Name: {insertedProduct.Name}, Price: {insertedProduct.Price}");

                return insertedProduct;
            }
            Console.WriteLine("❌ No Product Inserted.");
            return null; // If no row was inserted, return null
        }

        public async Task<Users> AddUsersAsync(Users user)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string query = @"
                                     INSERT INTO users (
                                     email, 
                                     password_hash, 
                                     first_name, 
                                     last_name, 
                                     created_at, 
                                     updated_at
                                    )
                                    OUTPUT INSERTED.*
                                    VALUES (
                                    @Email, 
                                    @PasswordHash, 
                                    @FirstName, 
                                    @LastName, 
                                    @CreatedAt, 
                                    @UpdatedAt
                                   )";

            try
            {
                using var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Email", user.email);
                command.Parameters.AddWithValue("@PasswordHash", user.password_hash);
                command.Parameters.AddWithValue("@FirstName", user.first_name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LastName", user.last_name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CreatedAt", user.created_at);
                command.Parameters.AddWithValue("@UpdatedAt", user.updated_at);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var insertedUser = new Users
                    {
                        user_id = reader.GetInt32(reader.GetOrdinal("user_id")),
                        email = reader.GetString(reader.GetOrdinal("email")),
                        password_hash = reader.GetString(reader.GetOrdinal("password_hash")),
                        first_name = reader.GetString("first_name"),
                        last_name = reader.GetString("last_name"),
                        created_at = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        updated_at = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                    };

                    return insertedUser;
                }
                return null; // If no row was inserted, return null
        }
            catch (SqlException ex)
            {
                throw; // Re-throw with original stack trace
            }
            finally
            {
                await connection.CloseAsync();
            }
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