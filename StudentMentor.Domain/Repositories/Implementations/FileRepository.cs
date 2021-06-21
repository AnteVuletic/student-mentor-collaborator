using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudentMentor.Data.Entities;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Repositories.Interfaces;
using File = StudentMentor.Data.Entities.Models.File;

namespace StudentMentor.Domain.Repositories.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly StudentMentorDbContext _dbContext;

        public FileRepository(StudentMentorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseResult<File>> AddFile(IFormFile formFile)
        {
            var file = new File
            {
                FileName = formFile.FileName
            };
            await _dbContext.Files.AddAsync(file);
            await _dbContext.SaveChangesAsync();

            var fileFolder = @$"files\{file.Id}\";
            var fileRootPath = fileFolder + file.FileName;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileFolder);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileRootPath);
            Directory.CreateDirectory(folderPath);
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);

            file.FilePath = fileRootPath;
            await _dbContext.SaveChangesAsync();
            return new ResponseResult<File>(file);
        }
    }
}
