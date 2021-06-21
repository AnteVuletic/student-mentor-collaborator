using System.Collections.Generic;

namespace StudentMentor.Data.Entities.Models.Github
{
    public class PushActivity
    {
        public int Id { get; set; }
        public string Ref { get; set; }
        public int RepositoryId { get; set; }
        public string RepositoryName { get; set; }
        
        public Message Message { get; set; }

        public ICollection<Commit> Commits { get; set; }
    }
}
