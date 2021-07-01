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
        public const string SendCommentMethod = "CommentRecieved";
        private readonly IMessageRepository _messageRepository;
        private readonly ICommentRepository _commentRepository;

        public MessagesHub(IMessageRepository messageRepository, ICommentRepository commentRepository)
        {
            _messageRepository = messageRepository;
            _commentRepository = commentRepository;
        }

        public async Task SendMessage(SendMessageModel message)
        {
            var userId = Context.UserIdentifier;
            var messageResponse = await _messageRepository.SendMessage(int.Parse(userId), message);
            var userToTask = Clients.User(message.UserToId.ToString()).SendAsync(SendMessageMethod, messageResponse);
            var userFromTask = Clients.User(userId).SendAsync(SendMessageMethod, messageResponse);

            Task.WaitAll(userToTask, userFromTask);
        }

        public async Task SendComment(CreateCommentModel comment)
        {
            var userId = Context.UserIdentifier;
            var userIdInt = int.Parse(userId);
            var commentResponse = await _commentRepository.Add(comment, userIdInt);
            if (commentResponse.IsError)
                return;

            var commentData = commentResponse.Data;
            var userToTask = Clients.User(commentData.UserMessageToId.ToString()).SendAsync(SendCommentMethod, commentData);
            var userFromTask = Clients.User(userId).SendAsync(SendCommentMethod, commentData);

            Task.WaitAll(userToTask, userFromTask);
        }
    }
}
