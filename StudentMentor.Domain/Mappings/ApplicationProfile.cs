using System.Linq;
using AutoMapper;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Data.Entities.Models.Github;
using StudentMentor.Data.Enums;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;

namespace StudentMentor.Domain.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            var baseUrl = "";
            CreateMap<User, UserModel>();

            CreateMap<Student, StudentModel>()
                .IncludeBase<User, UserModel>()
                .ForMember(src => src.Mentor, opts => opts.MapFrom(s => s.MentorId.HasValue
                    ? new UserModel
                    {
                        Id = s.Mentor.Id,
                        Email = s.Mentor.Email,
                        FirstName = s.Mentor.FirstName,
                        LastName = s.Mentor.LastName
                    }
                    : null))
                .ForMember(m => m.HasPaper, opts => opts.MapFrom(x => x.FinalPapers.Any()));

            CreateMap<Mentor, MentorModel>()
                .IncludeBase<User, UserModel>()
                .ForMember(src => src.Students, opts => opts.MapFrom(m => m.Students.Select(s => new UserModel
                {
                    Id = s.Id,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    LastName = s.LastName
                })));

            CreateMap<MentorInviteModel, Mentor>();
            CreateMap<Message, MessageModel>()
                .ForMember(m => m.UserFrom,
                    opts => opts.MapFrom(src => new UserModel
                    {
                        Id = src.UserFrom.Id,
                        Email = src.UserFrom.Email,
                        FirstName = src.UserFrom.FirstName,
                        LastName = src.UserFrom.LastName
                    }))
                .ForMember(m => m.UserTo,
                    opts => opts.MapFrom(src => new UserModel
                    {
                        Id = src.UserTo.Id,
                        Email = src.UserTo.Email,
                        FirstName = src.UserTo.FirstName,
                        LastName = src.UserTo.LastName
                    }))
                .ForMember(m => m.RepositoryName, src => src.MapFrom(x => x.PushActivity.RepositoryName))
                .ForMember(m => m.Commits, src => src.MapFrom(x => x.PushActivity.Commits.Select(c =>
                    new GithubMessageCommitModel
                    {
                        Id = c.Id,
                        Message = c.Message,
                        TimeStamp = c.TimeStamp,
                        Url = c.Url,
                        FileLogs = c.FileLogs.Select(fl => new GithubMessageFileLogModel
                        {
                            Id = fl.Id,
                            ChangeType = fl.ChangeType.ToString(),
                            File = fl.File
                        }).ToList()
                    })))
                .ForMember(m => m.File, src => src.MapFrom(x => x.FileId.HasValue
                    ? new FileModel
                    {
                        FileName = x.File.FileName,
                        Url = @$"{baseUrl}/{x.File.FilePath.Replace('\\', '/')}",
                        Id = x.File.Id,
                    }
                    : null))
                .ForMember(m => m.Comments, src => src.MapFrom(x => x.Comments.Select(c => new CommentModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserFrom = new UserModel
                    {
                        Id = c.User.Id,
                        Email = c.User.Email,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName
                    }
                })));

            CreateMap<MessageModel, Message>();

            CreateMap<SendMessageModel, Message>();

            CreateMap<PushModel, PushActivity>()
                .ForMember(m => m.RepositoryId, src => src.MapFrom(x => x.Repository.Id))
                .ForMember(m => m.RepositoryName, src => src.MapFrom(x => x.Repository.Name))
                .ForMember(m => m.Commits, src => src.MapFrom(x =>
                    x.Commits.Select(c => new Data.Entities.Models.Github.Commit
                    {
                        Id = c.Id,
                        Message = c.Message,
                        TimeStamp = c.TimeStamp,
                        Url = c.Url,
                        FileLogs = c
                            .Added.Select(a => new FileLog { ChangeType = FileChangeType.Added, File = a })
                            .Union(c.Modified.Select(m => new FileLog { ChangeType = FileChangeType.Changed, File = m }))
                            .Union(c.Removed.Select(r => new FileLog { ChangeType = FileChangeType.Deleted, File = r }))
                            .ToList()
                    }
                )));

            CreateMap<StudentFile, FileModelWithComments>()
                .ForMember(m => m.FileName, opts => opts.MapFrom(src => src.File.FileName))
                .ForMember(m => m.Id, opts => opts.MapFrom(src => src.File.Id))
                .ForMember(m => m.Url, opts => opts.MapFrom(src => @$"{baseUrl}/{src.File.FilePath.Replace('\\', '/')}"))
                .ForMember(m => m.Comments, opts => opts.MapFrom(src => src.File.Message.Comments.Select(c =>
                    new CommentModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        UserFrom = new UserModel
                        {
                            Id = c.User.Id,
                            Email = c.User.Email,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName
                        }
                    })));

            CreateMap<Comment, CommentModel>()
                .ForMember(c => c.UserFrom, opts => opts.MapFrom(src => new UserModel
                {
                    Id = src.User.Id,
                    Email = src.User.Email,
                    FirstName = src.User.FirstName,
                    LastName = src.User.LastName
                }))
                .ForMember(c => c.UserMessageToId,
                    opts => opts.MapFrom(src =>
                        src.Message.UserToId == src.UserId ? src.Message.UserFromId : src.Message.UserToId));
            CreateMap<CreateCommentModel, Comment>();
        }
    }
}
