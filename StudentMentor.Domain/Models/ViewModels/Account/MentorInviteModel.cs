using FluentValidation;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Resources;

namespace StudentMentor.Domain.Models.ViewModels.Account
{
    public class MentorInviteModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class MentorInviteModelValidation : AbstractValidator<MentorInviteModel>
    {
        public MentorInviteModelValidation(IUserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(userRepository.IsEmailAvailable)
                .WithMessage(ValidationMessages.EmailTaken);

            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }
}
