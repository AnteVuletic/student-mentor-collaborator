using FluentValidation;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Resources;

namespace StudentMentor.Domain.Models.ViewModels.Account
{
    public class RegistrationModelValidation : AbstractValidator<RegistrationModel>
    {
        private const int PasswordLength = 6;
        public RegistrationModelValidation(IUserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .MustAsync(userRepository.IsEmailTaken)
                .WithMessage(ValidationMessages.EmailTaken)
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(PasswordLength)
                .WithMessage(string.Format(ValidationMessages.PasswordShort, PasswordLength));
        }
    }
}
