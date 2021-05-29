using System.Threading;
using System.Threading.Tasks;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;

namespace StudentMentor.Domain.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailTaken(string email, CancellationToken token);
        Task<ResponseResult<User>> GetUserIfValidCredentials(LoginModel model);
        Task<User> GetUser(int userId);
        Task<UserModel> GetCurrentUserModel();
    }
}
