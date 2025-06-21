using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area42.Domain.Entities
{
    public class Medewerker
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
