using System.Threading.Tasks;
using Area42.Domain.Entities;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace Area42.Infrastructure.Data
{
    public class MedewerkerRepository : IMedewerkerRepository
    {
        private readonly string _connectionString;

        public MedewerkerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException("DefaultConnection");
        }

        public async Task<Medewerker?> GetMedewerkerByEmailAsync(string email)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            SELECT Id, Naam, Rol, Email, Password 
            FROM medewerker
            WHERE LOWER(Email) = @Email 
            LIMIT 1;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email.ToLower());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Medewerker
                            {
                                Id = reader.GetInt32("Id"),
                                Naam = reader["Naam"] as string ?? "",
                                Rol = reader["Rol"] as string ?? "",
                                Email = reader["Email"] as string ?? "",
                                Password = reader["Password"] as string ?? ""
                            };
                        }
                    }
                }
            }
            return null;
        }

    }
}
