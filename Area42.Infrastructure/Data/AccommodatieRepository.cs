using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Area42.Domain.Entities;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Area42.Infrastructure.Data
{
    public class AccommodatieRepository : IAccommodatieRepository
    {
        private readonly string _connectionString;

        public AccommodatieRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Accommodatie>> GetAllAccommodatiesAsync()
        {
            var accommodaties = new List<Accommodatie>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT 
                                    Id, 
                                    Naam, 
                                    `Type`, 
                                    Capaciteit, 
                                    Beschrijving, 
                                    PrijsPerNacht, 
                                    Faciliteiten, 
                                    Status 
                                 FROM Accommodaties";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            accommodaties.Add(new Accommodatie
                            {
                                Id = reader.GetInt32("Id"),
                                Naam = reader.GetString("Naam"),
                                Type = reader.GetString("Type"),
                                Capaciteit = reader.GetInt32("Capaciteit"),
                                Beschrijving = reader.GetString("Beschrijving"),
                                PrijsPerNacht = reader.GetDecimal("PrijsPerNacht"),
                                Faciliteiten = reader.GetString("Faciliteiten"),
                                Status = reader.GetString("Status")
                            });
                        }
                    }
                }
            }

            return accommodaties;
        }
    }
}