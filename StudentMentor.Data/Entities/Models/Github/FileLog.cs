using StudentMentor.Data.Enums;

namespace StudentMentor.Data.Entities.Models.Github
{
    public class FileLog
    {
        public int Id { get; set; }
        public string File { get; set; }
        public FileChangeType ChangeType { get; set; }
        public string CommitId { get; set; }
        public Commit Commit { get; set; }
    }
}
