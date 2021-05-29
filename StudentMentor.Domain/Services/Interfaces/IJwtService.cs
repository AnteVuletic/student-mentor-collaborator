using System.Threading.Tasks;
using StudentMentor.Data.Entities.Models;

namespace StudentMentor.Domain.Services.Interfaces
{
    public interface IJwtService
    {
        string GetJwtTokenForUser(User user);
        Task<string> GetNewToken(string token);
    }
}
