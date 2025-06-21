using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Area42.Domain.Entities;

public interface IMedewerkerService
{
    Task<Medewerker?> GetMedewerkerByEmailAsync(string email);
}
