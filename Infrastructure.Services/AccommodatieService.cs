using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AccommodatieService : IAccommodatieService
    {
        private readonly AppDbContext _context;

        public AccommodatieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Accommodatie>> GetAllAccommodatiesAsync()
        {
            return await _context.Accommodaties.ToListAsync();
        }

        // Vervolgimplementaties voor Create, Update, etc.
    }
}