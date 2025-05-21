using System.Data;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (var connection = new MySqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT Id, Username, Role FROM users WHERE LOWER(Username) = @username LIMIT 1;";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username.ToLower());

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            Role = reader.GetString("Role")
                        };
                    }
                }
            }
        }
        return null;
    }
}