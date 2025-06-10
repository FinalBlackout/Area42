using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Area42.Infrastructure.Services
{
    public class ReserveringService : IReserveringService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public ReserveringService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<Reservering>> GetReserveringenVoorUserAsync(ClaimsPrincipal user)
        {
            var reserveringen = new List<Reservering>();
            string query;
            bool isMedewerker = user.IsInRole("Medewerker");

            if (isMedewerker)
            {
                query = "SELECT Id, AccommodatieId, UserId, Startdatum, Einddatum, Status FROM Reserveringen;";
            }
            else
            {
                query = "SELECT Id, AccommodatieId, UserId, Startdatum, Einddatum, Status " +
                        "FROM Reserveringen WHERE UserId = (SELECT Id FROM users WHERE Username = @Username);";
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    if (!isMedewerker)
                    {
                        command.Parameters.AddWithValue("@Username", user.Identity.Name);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var reservering = new Reservering
                            {
                                Id = reader.GetInt32("Id"),
                                AccommodatieId = reader.GetInt32("AccommodatieId"),
                                UserId = reader.GetInt32("UserId"),
                                Startdatum = reader.GetDateTime("Startdatum"),
                                Einddatum = reader.GetDateTime("Einddatum"),
                                Status = reader.GetString("Status")
                            };
                            reserveringen.Add(reservering);
                        }
                    }
                }
            }
            return reserveringen;
        }

        public async Task CreateReserveringAsync(Reservering reservering)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"
                    INSERT INTO reserveringen (UserId, AccommodatieId, Startdatum, Einddatum, Status)
                    VALUES (@UserId, @AccommodatieId, @Startdatum, @Einddatum, @Status);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", reservering.UserId);
                    command.Parameters.AddWithValue("@AccommodatieId", reservering.AccommodatieId);
                    command.Parameters.AddWithValue("@Startdatum", reservering.Startdatum);
                    command.Parameters.AddWithValue("@Einddatum", reservering.Einddatum);
                    command.Parameters.AddWithValue("@Status", reservering.Status);

                    int affectedRows = await command.ExecuteNonQueryAsync();
                    System.Diagnostics.Debug.WriteLine($"Rows affected (insert): {affectedRows}");

                }
            }
        }

        public async Task<List<Reservering>> GetAllReserveringenAsync()
        {
            var reserveringen = new List<Reservering>();
            string query = "SELECT Id, AccommodatieId, UserId, Startdatum, Einddatum, Status FROM reserveringen;";

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var reservering = new Reservering
                            {
                                Id = reader.GetInt32("Id"),
                                AccommodatieId = reader.GetInt32("AccommodatieId"),
                                UserId = reader.GetInt32("UserId"),
                                Startdatum = reader.GetDateTime("Startdatum"),
                                Einddatum = reader.GetDateTime("Einddatum"),
                                Status = reader.GetString("Status")
                            };
                            reserveringen.Add(reservering);
                        }
                    }
                }
            }
            return reserveringen;
        }

        public async Task UpdateReserveringAsync(Reservering reservering)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    UPDATE reserveringen
                    SET AccommodatieId = @AccommodatieId,
                        UserId = @UserId,
                        Startdatum = @Startdatum,
                        Einddatum = @Einddatum,
                        Status = @Status
                    WHERE Id = @Id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccommodatieId", reservering.AccommodatieId);
                    command.Parameters.AddWithValue("@UserId", reservering.UserId);
                    command.Parameters.AddWithValue("@Startdatum", reservering.Startdatum);
                    command.Parameters.AddWithValue("@Einddatum", reservering.Einddatum);
                    command.Parameters.AddWithValue("@Status", reservering.Status);
                    command.Parameters.AddWithValue("@Id", reservering.Id);

                    int affectedRows = await command.ExecuteNonQueryAsync();
                    System.Diagnostics.Debug.WriteLine($"Rows affected (update): {affectedRows}");
                }
            }
        }

        public async Task DeleteReserveringAsync(int reserveringId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM reserveringen WHERE Id = @Id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", reserveringId);

                    int affectedRows = await command.ExecuteNonQueryAsync();
                    System.Diagnostics.Debug.WriteLine($"Rows affected (delete): {affectedRows}");
                }
            }
        }

        public async Task ApproveReserveringAsync(int reserveringId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE reserveringen SET Status = 'Goedgekeurd' WHERE Id = @Id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", MySqlDbType.Int32).Value = reserveringId;

                    int affectedRows = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Rows affected (approve): {affectedRows}");
                }
            }
        }

        public async Task RejectReserveringAsync(int reserveringId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE reserveringen SET Status = 'Afgewezen' WHERE Id = @Id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", reserveringId);

                    int affectedRows = await command.ExecuteNonQueryAsync();
                    System.Diagnostics.Debug.WriteLine($"Rows affected (reject): {affectedRows}");
                }
            }
        }
    }
}