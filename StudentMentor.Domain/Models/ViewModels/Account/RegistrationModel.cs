using FluentValidation;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Resources;

namespace StudentMentor.Domain.Models.ViewModels.Account
{
    public class RegistrationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegistrationModelValidation : AbstractValidator<RegistrationModel>
    {
        public const int PasswordLength = 6;
        public RegistrationModelValidation(IUserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .MustAsync(userRepository.IsEmailAvailable)
                .WithMessage(ValidationMessages.EmailTaken)
                .EmailAddress();

            RuleFor(x => x)
                .Must(x => x.Password == x.ConfirmPassword)
                .WithMessage(ValidationMessages.PasswordsMustMatch);

            RuleFor(x => x.Password)
                .MinimumLength(PasswordLength)
                .WithMessage(string.Format(ValidationMessages.PasswordShort, PasswordLength));
        }
    }
}
