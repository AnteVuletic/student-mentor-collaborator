using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;

namespace StudentMentor.Domain.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Task<ResponseResult<File>> AddFile(IFormFile file);
    }
}
