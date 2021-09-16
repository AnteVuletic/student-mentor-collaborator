using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;
using StudentMentor.Web.Infrastructure;

namespace StudentMentor.Web.Controllers
{
    [Authorize(Policy = Policies.Student)]
    public class GithubController : ApiController
    {
        private readonly IGithubService _githubService;
        private readonly IStudentRepository _studentRepository;

        public GithubController(IGithubService githubService, IStudentRepository studentRepository)
        {
            _githubService = githubService;
            _studentRepository = studentRepository;
        }

[HttpGet(nameof(GetAllRepositories))]
public async Task<ActionResult> GetAllRepositories()
{
    var response = await _githubService.GetAvailableRepositories();
    if (response.IsError)
        return BadRequest(response.Message);

    var repositoryId = await _studentRepository.GetRepositoryId();

    return Ok(new
    {
        repositoryId = repositoryId,
        repositories = response.Data
    });
}
    }
}
