using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Helpers;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Domain.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentMentorDbContext _dbContext;
        private readonly IWebHostService _webHostService;
        private readonly IClaimProvider _claimProvider;
        private readonly IMapper _mapper;

        public StudentRepository(StudentMentorDbContext dbContext, IWebHostService webHostService, IClaimProvider claimProvider, IMapper mapper)
        {
            _dbContext = dbContext;
            _webHostService = webHostService;
            _claimProvider = claimProvider;
            _mapper = mapper;
        }

        public async Task<ResponseResult<Student>> RegisterStudent(RegistrationModel model)
        {
            var password = EncryptionHelper.Hash(model.Password);
            var student = new Student
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = password
            };

            _dbContext.Add(student);
            await _dbContext.SaveChangesAsync();
            return new ResponseResult<Student>(student);
        }

        public async Task<ICollection<StudentModel>> GetStudents()
        {
            var students = await _dbContext.Students
                .AsNoTracking()
                .ProjectTo<StudentModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return students;
        }

        public async Task<ResponseResult<StudentModel>> GetStudent(int studentId)
        {
            var student = await _dbContext.Students
                .Where(s => s.Id == studentId)
                .ProjectTo<StudentModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return student is null
                ? ResponseResult<StudentModel>.Error("Not found")
                : new ResponseResult<StudentModel>(student);
        }

        public async Task<FileModel> GetFinalsPaper()
        {
            var baseUrl = _webHostService.GetRootUrl();
            var fileModel = await _dbContext
                .Students
                .Where(s => s.Id == _claimProvider.GetUserId() && s.FinalsPaperId.HasValue)
                .ProjectTo<FileModel>(_mapper.ConfigurationProvider, new {baseUrl})
                .FirstOrDefaultAsync();

            return fileModel;
        }

        public async Task<ResponseResult> DeleteStudent(int studentId)
        {
            var student = await _dbContext.Students.FindAsync(studentId);

            if (student is null)
                return ResponseResult.Error("Not found");

            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Ok;
        }

        public async Task<ResponseResult> SetMentor(int userId, int mentorId)
        {
            var student = await _dbContext.Students.FindAsync(userId);
            student.MentorId = mentorId;
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Ok;
        }

        public async Task<ResponseResult> RemoveMentor(int userId)
        {
            var student = await _dbContext.Students.FindAsync(userId);
            student.MentorId = null;
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Ok;
        }

        public async Task<ResponseResult> PatchGithubOAuthKey(string accessKey)
        {
            var student = await _dbContext.Students.FindAsync(_claimProvider.GetUserId());
            student.GithubAccessKey = accessKey;

            await _dbContext.SaveChangesAsync();
            return ResponseResult.Ok;
        }

        public async Task<ResponseResult> PatchGithubBearToken(string token)
        {
            var student = await _dbContext.Students.FindAsync(_claimProvider.GetUserId());
            student.GithubBearerToken = token;

            await _dbContext.SaveChangesAsync();
            return ResponseResult.Ok;
        }

        public async Task<ResponseResult<string>> GetOAuthKey()
        {
            var githubAccessKey = await _dbContext
                .Students
                .Where(s => s.Id == _claimProvider.GetUserId())
                .Select(s => s.GithubAccessKey)
                .FirstOrDefaultAsync();

            return new ResponseResult<string>(githubAccessKey);
        }

        public async Task<ResponseResult<Student>> PatchFinalsPaper(File file)
        {
            var student = await _dbContext.Students.FindAsync(_claimProvider.GetUserId());
            student.FinalsPaperId = file.Id;

            await _dbContext.SaveChangesAsync();
            return new ResponseResult<Student>(student);
        }

        public async Task<ResponseResult<string>> GetGitHubAccessToken()
        {
            var githubAccessKey = await _dbContext
                .Students
                .Where(s => s.Id == _claimProvider.GetUserId())
                .Select(s => s.GithubBearerToken)
                .FirstOrDefaultAsync();

            return new ResponseResult<string>(githubAccessKey);
        }

        public async Task<ResponseResult> PatchRepositoryId(int repositoryId)
        {
            var student = await _dbContext.Students.FindAsync(_claimProvider.GetUserId());
            student.GithubRepositoryId = repositoryId;

            await _dbContext.SaveChangesAsync();
            return ResponseResult.Ok;
        }

        public async Task<ICollection<UserModel>> GetMentoringStudents(int mentorId)
        {
            var students = await _dbContext
                .Students
                .Where(s => s.MentorId == mentorId)
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return students;
        }

        public async Task<int?> GetRepositoryId()
        {
            var repositoryId = await _dbContext
                .Students
                .Where(s => s.Id == _claimProvider.GetUserId())
                .Select(s => s.GithubRepositoryId)
                .FirstOrDefaultAsync();

            return repositoryId == 0 ? (int?) null : repositoryId;
        }
    }
}
