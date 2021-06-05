using System.Threading.Tasks;
using StudentMentor.Data.Entities.Models;

namespace StudentMentor.Domain.Services.Interfaces
{
    public interface IJwtService
    {
        string GetJwtTokenForUser(User user);
        string GetTokenForEmailInvite(int userId);
        int GetUserIdFromEmailToken(string emailInviteToken);
        Task<string> GetNewToken(string token);
    }
}
