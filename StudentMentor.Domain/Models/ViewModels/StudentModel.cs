namespace StudentMentor.Domain.Models.ViewModels
{
    public class StudentModel : UserModel
    {
        public UserModel Mentor { get; set; }
        public string GithubAccessKey { get; set; }
        public FileModel FinalsPaper { get; set; }
    }
}
