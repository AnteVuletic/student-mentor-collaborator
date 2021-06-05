namespace StudentMentor.EmailTemplates.Views.MentorInvite
{
    public class MentorInviteEmailModel : BaseEmailModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegistrationUrl { get; set; }
    }
}
