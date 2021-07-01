using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Resources;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Domain.Repositories.Implementations
{
    public class MessageRepository : IMessageRepository
    {
        private readonly StudentMentorDbContext _studentMentorDbContext;
        private readonly IClaimProvider _claimProvider;
        private readonly IWebHostService _webHostService;
        private readonly IMapper _mapper;

        public MessageRepository(StudentMentorDbContext studentMentorDbContext, IClaimProvider claimProvider, IWebHostService webHostService, IMapper mapper)
        {
            _studentMentorDbContext = studentMentorDbContext;
            _claimProvider = claimProvider;
            _webHostService = webHostService;
            _mapper = mapper;
        }

        public async Task<ICollection<MessageModel>> GetPage(int page, int pageSize, int? studentId)
        {
            var baseUrl = _webHostService.GetRootUrl();

            Expression<Func<Message, bool>> userFilterExpression = m =>
                m.UserFromId == _claimProvider.GetUserId() || m.UserToId == _claimProvider.GetUserId();

            if (studentId.HasValue)
            {
                userFilterExpression = m => m.UserFromId == studentId || m.UserToId == studentId;
            }

            var messages = await _studentMentorDbContext
                .Messages
                .Where(userFilterExpression)
                .OrderByDescending(m => m.MessageCreatedAt)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ProjectTo<MessageModel>(_mapper.ConfigurationProvider, new {baseUrl})
                .ToListAsync();
            return messages;
        }

        public async Task<ResponseResult<MessageModel>> SendMessage(int fromId, SendMessageModel model)
        {
            var message = _mapper.Map<Message>(model);

            message.UserFromId = fromId;
            message.MessageCreatedAt = DateTime.Now;
            await _studentMentorDbContext.Messages.AddAsync(message);
            await _studentMentorDbContext.SaveChangesAsync();

            var messageResponse = await _studentMentorDbContext
                .Messages
                .Where(m => m.Id == message.Id)
                .ProjectTo<MessageModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();


            return new ResponseResult<MessageModel>(messageResponse);
        }

        public async Task<ResponseResult<MessageModel>> SendGithubMessage(PushActivityEvent model)
        {
            var pushActivity = await _studentMentorDbContext.PushActivities.FindAsync(model.Id);

            var studentWithRepository =
                await _studentMentorDbContext.Students.FirstOrDefaultAsync(s =>
                    s.GithubRepositoryId == pushActivity.RepositoryId);

            var message = new Message
            {
                MessageCreatedAt = DateTime.Now,
                PushActivityId = pushActivity.Id,
                Content = "Github notification",
                UserFromId = studentWithRepository.Id,
                UserToId = studentWithRepository.MentorId,
            };

            await _studentMentorDbContext.Messages.AddAsync(message);
            await _studentMentorDbContext.SaveChangesAsync();
            var messageModel = await GetMessageById(message.Id);
            return new ResponseResult<MessageModel>(messageModel);
        }


        public async Task<ResponseResult<MessageModel>> SendFileMessage(File file, int userFromId, int userToId)
        {
            var message = new Message
            {
                UserToId = userToId,
                UserFromId = userFromId,
                FileId = file.Id,
                MessageCreatedAt = DateTime.Now
            };

            await _studentMentorDbContext.Messages.AddAsync(message);
            await _studentMentorDbContext.SaveChangesAsync();

            var messageModel = await GetMessageById(message.Id);

            return new ResponseResult<MessageModel>(messageModel);
        }

        public async Task<MessageModel> GetMessageById(int id)
        {
            var baseUrl = _webHostService.GetRootUrl();
            var message = await _studentMentorDbContext
                .Messages
                .Where(m => m.Id == id)
                .ProjectTo<MessageModel>(_mapper.ConfigurationProvider, new { baseUrl })
                .FirstOrDefaultAsync();

            return message;
        }

        public async Task<ResponseResult> DeleteMessage(int messageId)
        {
            var message = await _studentMentorDbContext
                .Messages
                .FindAsync(messageId);

            if (message == null)
                return ResponseResult.Error(ValidationMessages.MessageNotFound);

            return ResponseResult.Ok;
        }
    }
}
