using System.Threading.Tasks;
using AutoMapper;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Entities.Models.Github;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;

namespace StudentMentor.Domain.Repositories.Implementations
{
    public class GithubRepository : IGithubRepository
    {
        private readonly StudentMentorDbContext _dbContext;
        private readonly IMapper _mapper;

        public GithubRepository(StudentMentorDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResponseResult<PushActivityEvent>> SaveGithubPushEvent(PushModel model)
        {
            var mapped = _mapper.Map<PushActivity>(model);
            await _dbContext.PushActivities.AddAsync(mapped);
            await _dbContext.SaveChangesAsync();

            return new ResponseResult<PushActivityEvent>(new PushActivityEvent { Id = mapped.Id });
        }
    }
}
