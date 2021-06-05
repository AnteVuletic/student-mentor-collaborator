using System.Threading.Tasks;
using StudentMentor.Domain.Models.ViewModels;

namespace StudentMentor.Domain.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmail(EmailMessage message);
    }
}
