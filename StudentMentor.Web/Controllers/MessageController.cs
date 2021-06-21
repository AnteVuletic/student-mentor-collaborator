using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Web.Controllers
{
    public class MessageController : ApiController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IClaimProvider _claimProvider;

        public MessageController(IMessageRepository messageRepository, IClaimProvider claimProvider)
        {
            _messageRepository = messageRepository;
            _claimProvider = claimProvider;
        }

        [HttpGet]
        public async Task<ActionResult<MessageModel>> GetPaged([FromQuery] int page, [FromQuery] int pageSize)
        {
            var messages = await _messageRepository.GetPage(page, pageSize);

            return Ok(messages);
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(SendMessageModel model)
        {
            var response = await _messageRepository.SendMessage(_claimProvider.GetUserId(), model);

            if (response.IsError)
                return BadRequest(response.Message);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage([FromRoute] int id)
        {
            var response = await _messageRepository.DeleteMessage(id);

            if (response.IsError)
                return NotFound(response.Message);

            return Ok();
        }
    }
}
