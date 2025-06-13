using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Area42.Domain.Entities;

namespace Area42.Domain.Entities
{
    public class Reservering
    {
        public int Id { get; set; }
        public int AccommodatieId { get; set; }
        public int UserId { get; set; }
        public DateTime Startdatum { get; set; }
        public DateTime Einddatum { get; set; }
        public string Status { get; set; } //Bevestigd, In behandeling, Geannuleerd
    }
}