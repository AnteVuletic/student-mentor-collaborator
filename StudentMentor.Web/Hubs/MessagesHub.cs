using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;

namespace StudentMentor.Web.Hubs
{
    [Authorize]
    public class MessagesHub : Hub
    {
        public const string SendMessageMethod = "MessageRecieved";
        private readonly IMessageRepository _messageRepository;

        public MessagesHub(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessage(SendMessageModel message)
        {
            var userId = Context.UserIdentifier;
            var messageResponse = await _messageRepository.SendMessage(int.Parse(userId), message);
            var userToTask = Clients.User(message.UserToId.ToString()).SendAsync(SendMessageMethod, messageResponse);
            var userFromTask = Clients.User(userId).SendAsync(SendMessageMethod, messageResponse);

            Task.WaitAll(userToTask, userFromTask);
        }
    }
}
