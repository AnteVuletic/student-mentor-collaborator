using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;
using StudentMentor.Domain.Abstractions;

namespace StudentMentor.Domain.Services.Interfaces
{
    public interface IGithubService
    {
        string GetOAuthLink();
        Task<ResponseResult<IReadOnlyCollection<Repository>>> GetAvailableRepositories();
        Task<string> GetBearerToken(string oAuthToken = null);
        Task<ResponseResult<RepositoryHook>> CreateWebhookForRepositoryId(long repositoryId);
    }
}
