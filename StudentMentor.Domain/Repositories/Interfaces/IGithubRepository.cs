using System.Threading.Tasks;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;

namespace StudentMentor.Domain.Repositories.Interfaces
{
    public interface IGithubRepository
    {
        Task<ResponseResult<PushActivityEvent>> SaveGithubPushEvent(PushModel model);
    }
}
