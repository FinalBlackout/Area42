using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Area42.Infrastructure.Services
{
    public class ReserveringService : IReserveringService
    {

        private readonly string _connectionString;

        public ReserveringService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Reservering>> GetReserveringenVoorUserAsync(ClaimsPrincipal user)
        {
            var reserveringen = new List<Reservering>();

            // Basisquery: als medewerker, haal alle reserveringen op.
            // Als klant, filter dan op de gebruikersnaam of een ander uniek kenmerk.
            string query;
            bool isMedewerker = user.IsInRole("Medewerker");

            if (isMedewerker)
            {
                query = "SELECT Id, AccommodatieId, UserId, Startdatum, Einddatum, Status FROM Reserveringen";
            }
            else
            {
                // Veronderstel dat je de gebruikersnaam als identificatie voor een klant gebruikt.
                // In een productie-implementatie zou je wellicht een klant-ID in de Users-tabel vastleggen.
                string username = user.Identity.Name;
                query = "SELECT Id, AccommodatieId, UserId, Startdatum, Einddatum, Status " +
                        "FROM Reserveringen WHERE UserId = (SELECT Id FROM Klanten WHERE Gebruikersnaam = @Username) ";
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



        public Task CreateReserveringAsync(Reservering reservering)
        {
            // Voeg logica toe om een reservering op te slaan in de database.
            return Task.CompletedTask;
        }

        public Task<List<Reservering>> GetAllReserveringenAsync()
        {
            // Retourneer een dummy lijst met reserveringen.
            return Task.FromResult(new List<Reservering>());
        }

        public Task UpdateReserveringAsync(Reservering reservering)
        {
            // Voeg logica toe om een reservering bij te werken.
            return Task.CompletedTask;
        }

        public Task DeleteReserveringAsync(int reserveringId)
        {
            // Voeg logica toe voor het verwijderen van de reservering.
            return Task.CompletedTask;
        }

        public Task ApproveReserveringAsync(int reserveringId)
        {
            // Logica om een reservering goed te keuren.
            return Task.CompletedTask;
        }

        public Task RejectReserveringAsync(int reserveringId)
        {
            // Logica om een reservering af te wijzen.
            return Task.CompletedTask;
        }
    }
}