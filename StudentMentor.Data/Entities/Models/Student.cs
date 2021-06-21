namespace StudentMentor.Data.Entities.Models
{
    public class Student : User
    {
        public string GithubAccessKey { get; set; }
        public string GithubBearerToken { get; set; }
        public int GithubRepositoryId { get; set; }

        public int? FinalsPaperId { get; set; }
        public File FinalsPaper { get; set; }

        public int? MentorId { get; set; }
        public Mentor Mentor { get; set; }
    }
}
