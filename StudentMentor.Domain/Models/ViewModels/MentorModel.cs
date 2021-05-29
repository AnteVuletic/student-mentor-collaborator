using System.Collections.Generic;

namespace StudentMentor.Domain.Models.ViewModels
{
    public class MentorModel : UserModel
    {
        public ICollection<UserModel> Students { get; set; }
    }
}
