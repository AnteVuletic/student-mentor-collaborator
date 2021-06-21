using System;
using System.Collections.Generic;

namespace StudentMentor.Data.Entities.Models.Github
{
    public class Commit
    {
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }

        public int PushActivityId { get; set; }
        public PushActivity PushActivity { get; set; }

        public ICollection<FileLog> FileLogs { get; set; }
    }
}
