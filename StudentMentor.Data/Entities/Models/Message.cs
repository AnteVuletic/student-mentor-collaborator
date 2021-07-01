using System;
using System.Collections.Generic;
using StudentMentor.Data.Entities.Models.Github;

namespace StudentMentor.Data.Entities.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? UserFromId { get; set; }
        public User UserFrom { get; set; }

        public int? UserToId { get; set; }
        public User UserTo { get; set; }

        public int? PushActivityId { get; set; }
        public PushActivity PushActivity { get; set; }

        public int? FileId { get; set; }
        public File File { get; set; }

        public DateTime MessageCreatedAt { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
