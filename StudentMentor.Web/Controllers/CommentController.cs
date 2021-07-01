using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;

namespace StudentMentor.Web.Controllers
{
    public class CommentController : ApiController
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(CreateCommentModel model)
        {
            var response = await _commentRepository.Add(model);

            if (response.IsError)
            {
                return BadRequest(response.Message);
            }

            return Ok();
        }
    }
}
