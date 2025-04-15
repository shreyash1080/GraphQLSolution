using Core.Entities;
using Core.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly string _connectionString;


        /// <summary>
        /// Constructor to initialize the repository with a database connection string.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Users?> GetUserLoginAsync(string email, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string query = @"
                                    SELECT user_id,email,first_name,last_name FROM users 
                                    WHERE email = @Email AND password_hash = @PasswordHash";
            try
            {

                using var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", password);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Users
                    {
                        user_id = reader.GetInt32(reader.GetOrdinal("user_id")),
                        email = reader.GetString(reader.GetOrdinal("email")),
                        first_name = reader.GetString(reader.GetOrdinal("first_name")),
                        last_name = reader.GetString(reader.GetOrdinal("last_name"))

                    };

                }
                return null;

            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while executing the SQL command.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
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
                throw new Exception("This is SQL Exception ${ex}",ex); // Re-throw with original stack trace
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

    }
}
