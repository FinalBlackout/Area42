using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Area42.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Area42.Infrastructure.Data
{
    public class AccommodatieRepository : IAccommodatieRepository
    {
        private readonly string _cs;

        // Constructor that initializes the connection string from configuration
        public AccommodatieRepository(IConfiguration configuration)
        {
            _cs = configuration.GetConnectionString("DefaultConnection");
        }

        // Gets a list of all accommodations from the database
        public async Task<List<Accommodatie>> GetAllAccommodatiesAsync()
        {
            var lijst = new List<Accommodatie>();
            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();

            const string sql = @"
                SELECT Id, Naam, `Type`, Capaciteit,
                       Beschrijving, PrijsPerNacht,
                       Faciliteiten, Status, ImagePath
                  FROM accommodaties";
            await using var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lijst.Add(new Accommodatie
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
                });
            }

            return lijst;
        }

        // Gets a specific accommodation by its ID
        public async Task AddAccommodatieAsync(Accommodatie accommodatie)
        {
            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();

            const string sql = @"
                INSERT INTO accommodaties
                  (Naam, `Type`, Capaciteit, Beschrijving,
                   PrijsPerNacht, Faciliteiten, Status, ImagePath)
                VALUES
                  (@Naam, @Type, @Capaciteit, @Beschrijving,
                   @PrijsPerNacht, @Faciliteiten, @Status, @ImagePath)";
            await using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Naam", accommodatie.Naam);
            cmd.Parameters.AddWithValue("@Type", accommodatie.Type);
            cmd.Parameters.AddWithValue("@Capaciteit", accommodatie.Capaciteit);
            cmd.Parameters.AddWithValue(
              "@Beschrijving",
              (object)accommodatie.Beschrijving ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PrijsPerNacht", accommodatie.PrijsPerNacht);
            cmd.Parameters.AddWithValue(
              "@Faciliteiten",
              (object)accommodatie.Faciliteiten ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", accommodatie.Status);
            cmd.Parameters.AddWithValue(
              "@ImagePath",
              string.IsNullOrEmpty(accommodatie.ImagePath)
                ? (object)DBNull.Value
                : accommodatie.ImagePath);

            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows == 0)
                throw new Exception("Kon accommodatie niet toevoegen.");
        }

        // Gets a specific accommodation by its ID
        public async Task DeleteAccommodatieAsync(int id)
        {
            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();

            const string sql = "DELETE FROM accommodaties WHERE Id = @Id";
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows == 0)
                throw new KeyNotFoundException($"Geen accommodatie met ID {id} gevonden.");
        }
    }
}