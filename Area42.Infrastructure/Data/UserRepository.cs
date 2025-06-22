using System.Threading.Tasks;
using Area42.Domain.Entities;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
namespace Area42.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        // Connection string to the MySQL database, initialized from configuration
        private readonly string _connectionString;
        // Constructor that initializes the connection string from configuration
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException("DefaultConnection");
        }

        // Gets a user by their email address
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                SELECT 
                    Id, name, email, password, address, telephone, job, status 
                FROM user
                WHERE LOWER(email) = @Email 
                LIMIT 1;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email.ToLower());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("Id"),
                                name = reader["name"]?.ToString() ?? "",
                                email = reader["email"]?.ToString() ?? "",
                                password = reader["password"]?.ToString() ?? "",
                                address = reader["address"]?.ToString() ?? "",
                                telephone = reader.GetInt32("telephone"),
                                job = reader["job"]?.ToString() ?? "",
                                status = reader["status"]?.ToString() ?? ""
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Gets a user by their username (name in this case)
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            SELECT 
                Id, name, email, password, address, telephone, job, status 
            FROM user
            WHERE LOWER(name) = @Username 
            LIMIT 1;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username.ToLower());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("Id"),
                                name = reader["name"]?.ToString() ?? "",
                                email = reader["email"]?.ToString() ?? "",
                                password = reader["password"]?.ToString() ?? "",
                                address = reader["address"]?.ToString() ?? "",
                                telephone = reader.GetInt32("telephone"),
                                job = reader["job"]?.ToString() ?? "",
                                status = reader["status"]?.ToString() ?? ""
                            };
                        }
                    }
                }
            }
            return null;
        }


    } }