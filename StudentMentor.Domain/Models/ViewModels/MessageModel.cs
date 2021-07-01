using System;
using System.Collections.Generic;
using StudentMentor.Data.Enums;

namespace StudentMentor.Domain.Models.ViewModels
{
    public class MessageModel
    {
        public int Id { get; set; }
        public UserModel UserFrom { get; set; }
        public UserModel UserTo { get; set; }
        public string Content { get; set; }
        public DateTime MessageCreatedAt { get; set; }
        public string RepositoryName { get; set; }
        public string Ref { get; set; }
        public FileModel File { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
        public ICollection<GithubMessageCommitModel> Commits { get; set; }
    }

    public class GithubMessageCommitModel
    {
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public ICollection<GithubMessageFileLogModel> FileLogs { get; set; }
    }

    public class GithubMessageFileLogModel
    {
        public int Id { get; set; }
        public string File { get; set; }
        public FileChangeType ChangeType { get; set; }
    }

    public class FileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }

    public class FileModelWithComments : FileModel
    {
        public ICollection<CommentModel> Comments { get; set; }
    }
}
