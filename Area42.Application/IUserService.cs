using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Area42.Domain.Entities;

namespace Area42.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
