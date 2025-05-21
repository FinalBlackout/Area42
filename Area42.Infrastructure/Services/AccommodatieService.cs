using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Query inclusief de ImagePath kolom indien beschikbaar
                    string query = @"
                        SELECT 
                            Id, 
                            Naam, 
                            Type, 
                            Capaciteit, 
                            Beschrijving, 
                            PrijsPerNacht, 
                            Faciliteiten, 
                            Status,
                            ImagePath 
                        FROM accommodaties";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var accommodatie = new Accommodatie
                                {
                                    Id = reader.GetInt32("Id"),
                                    Naam = reader.GetString("Naam"),
                                    Type = reader.GetString("Type"),
                                    Capaciteit = reader.GetInt32("Capaciteit"),
                                    // Controleer op NULL-waardes voor optionele tekstvelden
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

                                accommodaties.Add(accommodatie);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hier kun je eventueel via een logging framework de fout loggen.
                // Voor nu schrijven we de foutmelding in de Debug output.
                System.Diagnostics.Debug.WriteLine("Fout bij het ophalen van accommodaties: " + ex.Message);

                // In een productieomgeving kun je eventueel de exception opnieuw gooien of een fallback-mechanisme toepassen.
                throw;
            }

            return accommodaties;
        }


        public async Task AddAccommodatieAsync(Accommodatie accommodatie)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        INSERT INTO accommodaties 
                        (Naam, Type, Capaciteit, Beschrijving, PrijsPerNacht, Faciliteiten, Status, ImagePath)
                        VALUES 
                        (@Naam, @Type, @Capaciteit, @Beschrijving, @PrijsPerNacht, @Faciliteiten, @Status, @ImagePath)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Naam", accommodatie.Naam);
                        command.Parameters.AddWithValue("@Type", accommodatie.Type);
                        command.Parameters.AddWithValue("@Capaciteit", accommodatie.Capaciteit);
                        command.Parameters.AddWithValue("@Beschrijving", accommodatie.Beschrijving);
                        command.Parameters.AddWithValue("@PrijsPerNacht", accommodatie.PrijsPerNacht);
                        command.Parameters.AddWithValue("@Faciliteiten", accommodatie.Faciliteiten);
                        command.Parameters.AddWithValue("@Status", accommodatie.Status);
                        command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(accommodatie.ImagePath)
                            ? (object)DBNull.Value
                            : accommodatie.ImagePath);

                        int affectedRows = await command.ExecuteNonQueryAsync();
                        System.Diagnostics.Debug.WriteLine($"Aantal toegevoegde rijen: {affectedRows}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fout bij toevoegen accommodatie: " + ex.Message);
                throw;  // Overweeg om de fout door te geven, dan zie je hem tijdens het debuggen
            }
        }
    }
}