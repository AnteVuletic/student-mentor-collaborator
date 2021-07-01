namespace StudentMentor.Domain.Models.ViewModels
{
    public class StudentModel : UserModel
    {
        public UserModel Mentor { get; set; }
        public string GithubAccessKey { get; set; }
        public bool HasPaper { get; set; }
    }
}
