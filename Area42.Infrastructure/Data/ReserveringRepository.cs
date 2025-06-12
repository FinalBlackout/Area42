using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Area42.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Area42.Infrastructure.Data
{
    public class ReserveringRepository : IReserveringRepository
    {
        private readonly string _cs;
        public ReserveringRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection")
                  ?? throw new ArgumentNullException("DefaultConnection");
        }

        public async Task<List<Reservering>> GetReserveringenAsync(string userId = null)
        {
            var lijst = new List<Reservering>();
            // Basis-query
            var sql = @"
                SELECT Id, AccommodatieId, UserId, Startdatum, Einddatum, Status 
                  FROM reserveringen";
            // Als userId is meegegeven, filter dan
            if (!string.IsNullOrEmpty(userId))
                sql += " WHERE UserId = @UserId;";

            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);

            if (!string.IsNullOrEmpty(userId))
                cmd.Parameters.AddWithValue("@UserId", userId);

            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                lijst.Add(new Reservering
                {
                    Id = rdr.GetInt32("Id"),
                    AccommodatieId = rdr.GetInt32("AccommodatieId"),
                    UserId = rdr.GetInt32("UserId"),
                    Startdatum = rdr.GetDateTime("Startdatum"),
                    Einddatum = rdr.GetDateTime("Einddatum"),
                    Status = rdr.GetString("Status")
                });
            }
            return lijst;
        }

        public async Task<Reservering> GetReserveringByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, AccommodatieId, UserId, Startdatum, Einddatum, Status
                  FROM reserveringen
                 WHERE Id = @Id;";
            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await using var rdr = await cmd.ExecuteReaderAsync();
            if (!await rdr.ReadAsync())
                return null;

            return new Reservering
            {
                Id = rdr.GetInt32("Id"),
                AccommodatieId = rdr.GetInt32("AccommodatieId"),
                UserId = rdr.GetInt32("UserId"),
                Startdatum = rdr.GetDateTime("Startdatum"),
                Einddatum = rdr.GetDateTime("Einddatum"),
                Status = rdr.GetString("Status")
            };
        }

        public async Task AddAsync(Reservering r)
        {
            const string sql = @"
                INSERT INTO reserveringen
                  (UserId, AccommodatieId, Startdatum, Einddatum, Status)
                VALUES
                  (@UserId, @AccommodatieId, @Startdatum, @Einddatum, @Status);";

            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", r.UserId);
            cmd.Parameters.AddWithValue("@AccommodatieId", r.AccommodatieId);
            cmd.Parameters.AddWithValue("@Startdatum", r.Startdatum);
            cmd.Parameters.AddWithValue("@Einddatum", r.Einddatum);
            cmd.Parameters.AddWithValue("@Status", r.Status);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Reservering r)
        {
            const string sql = @"
                UPDATE reserveringen
                   SET AccommodatieId = @AccommodatieId,
                       UserId         = @UserId,
                       Startdatum     = @Startdatum,
                       Einddatum      = @Einddatum,
                       Status         = @Status
                 WHERE Id = @Id;";

            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", r.Id);
            cmd.Parameters.AddWithValue("@AccommodatieId", r.AccommodatieId);
            cmd.Parameters.AddWithValue("@UserId", r.UserId);
            cmd.Parameters.AddWithValue("@Startdatum", r.Startdatum);
            cmd.Parameters.AddWithValue("@Einddatum", r.Einddatum);
            cmd.Parameters.AddWithValue("@Status", r.Status);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM reserveringen WHERE Id = @Id;";
            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows == 0)
                throw new KeyNotFoundException($"Geen reservering met ID {id} gevonden.");
        }

        public async Task ApproveAsync(int id)
        {
            const string sql = "UPDATE reserveringen SET Status = 'Goedgekeurd' WHERE Id = @Id;";
            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RejectAsync(int id)
        {
            const string sql = "UPDATE reserveringen SET Status = 'Afgewezen' WHERE Id = @Id;";
            await using var conn = new MySqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}