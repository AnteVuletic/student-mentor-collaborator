using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Web.Hubs;

namespace StudentMentor.Web.Controllers
{
    [AllowAnonymous]
    public class HookController : ApiController
    {
        private readonly IGithubRepository _githubRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<MessagesHub> _hubContext;

        public HookController(IGithubRepository githubRepository, IMessageRepository messageRepository, IHubContext<MessagesHub> hubContext)
        {
            _githubRepository = githubRepository;
            _messageRepository = messageRepository;
            _hubContext = hubContext;
        }

        [HttpPost(nameof(Push))]
        public async Task<ActionResult> Push(PushModel model)
        {
            var response = await _githubRepository.SaveGithubPushEvent(model);
            if (response.IsError)
                return NoContent();
            var messageResponse = await _messageRepository.SendGithubMessage(response.Data);

            if (messageResponse.IsError)
                return NoContent();

            var message = messageResponse.Data;
            await _hubContext.Clients.Users(message.UserTo.Id.ToString(), message.UserFrom.Id.ToString()).SendAsync(MessagesHub.SendMessageMethod, response.Data);

            return Ok();
        }
    }
}
