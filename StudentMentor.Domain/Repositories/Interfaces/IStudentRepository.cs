using System.Collections.Generic;
using System.Threading.Tasks;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;

namespace StudentMentor.Domain.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<ResponseResult<Student>> RegisterStudent(RegistrationModel model);
        Task<ICollection<StudentModel>> GetStudents();
        Task<ResponseResult<StudentModel>> GetStudent(int studentId);
        Task<FileModel> GetFinalsPaper();
        Task<ResponseResult> DeleteStudent(int studentId);
        Task<ResponseResult> SetMentor(int userId, int mentorId);
        Task<ResponseResult> RemoveMentor(int userId);
        Task<ResponseResult> PatchGithubOAuthKey(string accessKey);
        Task<ResponseResult<string>> GetOAuthKey();
        Task<ResponseResult> PatchGithubBearToken(string token);
        Task<ResponseResult> PatchRepositoryId(int repositoryId);
        Task<ResponseResult<Student>> PatchFinalsPaper(File file);
        Task<ResponseResult<string>> GetGitHubAccessToken();
        Task<ICollection<UserModel>> GetMentoringStudents(int mentorId);
        Task<int?> GetRepositoryId();
    }
}
