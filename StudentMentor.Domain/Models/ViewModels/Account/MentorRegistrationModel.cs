using FluentValidation;
using StudentMentor.Domain.Resources;

namespace StudentMentor.Domain.Models.ViewModels.Account
{
    public class MentorRegistrationModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class MentorRegistrationModelValidation : AbstractValidator<MentorRegistrationModel>
    {
        public MentorRegistrationModelValidation()
        {
            RuleFor(x => x)
                .Must(x => x.Password == x.ConfirmPassword)
                .WithMessage(ValidationMessages.PasswordsMustMatch);

            RuleFor(x => x.Password)
                .MinimumLength(RegistrationModelValidation.PasswordLength)
                .WithMessage(string.Format(ValidationMessages.PasswordShort, RegistrationModelValidation.PasswordLength));
        }
    }
}
