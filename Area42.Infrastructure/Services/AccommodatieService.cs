using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace Area42.Infrastructure.Services
{
    public class AccommodatieService : IAccommodatieService
    {
        private readonly string _connectionString;

        public AccommodatieService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Accommodatie>> GetAllAccommodatiesAsync()
        {
            var accommodaties = new List<Accommodatie>();

            try
            {
                await using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                const string query = @"
                    SELECT 
                        Id, Naam, Type, Capaciteit, Beschrijving, 
                        PrijsPerNacht, Faciliteiten, Status, ImagePath
                    FROM accommodaties";

                await using var command = new MySqlCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var acc = new Accommodatie
                    {
                        Id = reader.GetInt32("Id"),
                        Naam = reader.GetString("Naam"),
                        Type = reader.GetString("Type"),
                        Capaciteit = reader.GetInt32("Capaciteit"),
                        Beschrijving = reader.IsDBNull(reader.GetOrdinal("Beschrijving"))
                                      ? null
                                      : reader.GetString("Beschrijving"),
                        PrijsPerNacht = reader.GetDecimal("PrijsPerNacht"),
                        Faciliteiten = reader.IsDBNull(reader.GetOrdinal("Faciliteiten"))
                                      ? null
                                      : reader.GetString("Faciliteiten"),
                        Status = reader.GetString("Status"),
                        ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath"))
                                    ? null
                                    : reader.GetString("ImagePath")
                    };
                    accommodaties.Add(acc);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return accommodaties;
        }

        public async Task AddAccommodatieAsync(Accommodatie accommodatie)
        {
            try
            {
                await using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                const string query = @"
                    INSERT INTO accommodaties
                        (Naam, Type, Capaciteit, Beschrijving, PrijsPerNacht, Faciliteiten, Status, ImagePath)
                    VALUES
                        (@Naam, @Type, @Capaciteit, @Beschrijving, @PrijsPerNacht, @Faciliteiten, @Status, @ImagePath)";

                await using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Naam", accommodatie.Naam);
                command.Parameters.AddWithValue("@Type", accommodatie.Type);
                command.Parameters.AddWithValue("@Capaciteit", accommodatie.Capaciteit);
                command.Parameters.AddWithValue("@Beschrijving", (object)accommodatie.Beschrijving ?? DBNull.Value);
                command.Parameters.AddWithValue("@PrijsPerNacht", accommodatie.PrijsPerNacht);
                command.Parameters.AddWithValue("@Faciliteiten", (object)accommodatie.Faciliteiten ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", accommodatie.Status);
                command.Parameters.AddWithValue("@ImagePath",
                    string.IsNullOrEmpty(accommodatie.ImagePath) ? (object)DBNull.Value : accommodatie.ImagePath);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteAccommodatieAsync(int id)
        {
            try
            {
                await using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                const string query = @"DELETE FROM accommodaties WHERE Id = @Id";

                await using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                var affected = await command.ExecuteNonQueryAsync();
                if (affected == 0)
                {
                    throw new KeyNotFoundException($"Geen accommodatie gevonden met ID {id}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}