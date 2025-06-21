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
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException("DefaultConnection");
        }

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