using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMentor.Domain.Repositories.Interfaces;

namespace StudentMentor.Web.Controllers
{
    public class FileController : ApiController
    {
        private readonly IFileRepository _fileRepository;

        public FileController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        [HttpPost]
        public async Task<ActionResult> PostFile(IFormFile file)
        {
            var fileResponse = await _fileRepository.AddFile(file);

            return Ok(fileResponse.Data);
        }
    }
}
