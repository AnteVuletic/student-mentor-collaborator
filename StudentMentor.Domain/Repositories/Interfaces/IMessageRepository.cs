using System.Collections.Generic;
using System.Threading.Tasks;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;

namespace StudentMentor.Domain.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<ICollection<MessageModel>> GetPage(int page, int pageSize, int? studentId);
        Task<ResponseResult<MessageModel>> SendMessage(int fromId, SendMessageModel model);
        Task<ResponseResult<MessageModel>> SendGithubMessage(PushActivityEvent model);
        Task<MessageModel> GetMessageById(int id);
        Task<ResponseResult<MessageModel>> SendFileMessage(File file, int userFromId, int userToId);
        Task<ResponseResult> DeleteMessage(int messageId);
    }
}
