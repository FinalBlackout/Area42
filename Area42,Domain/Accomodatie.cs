using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area42.Domain.Entities
{
    public class Accommodatie
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Type { get; set; }
        public int Capaciteit { get; set; }
        public string Beschrijving { get; set; }
        public decimal PrijsPerNacht { get; set; }
        public string Faciliteiten { get; set; }
        public string Status { get; set; } // Bijvoorbeeld "Beschikbaar", "Bezet", "Onderhoud"
        public string ImagePath { get; set; }

    }
}
