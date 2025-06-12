using System.Threading.Tasks;
using Area42.Domain.Entities;

namespace Area42.Infrastructure.Data
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
    }
}