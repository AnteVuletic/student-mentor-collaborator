using System.Collections.Generic;
using System.Threading.Tasks;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Services.Interfaces;
using StudentMentor.EmailTemplates;
using StudentMentor.EmailTemplates.Views.MentorInvite;

namespace StudentMentor.Web.Infrastructure.Email
{
    public class EmailHandler
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IWebHostService _webHostService;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        public EmailHandler(
            IEmailSenderService emailSenderService,
            IWebHostService webHostService,
            IRazorViewToStringRenderer razorViewToStringRenderer
        ) {
            _emailSenderService = emailSenderService;
            _webHostService = webHostService;
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }

        public async Task SendMentorInviteEmail(Mentor mentor, string token)
        {
            var mentorInviteEmailModel = new MentorInviteEmailModel
            {
                FirstName = mentor.FirstName,
                LastName = mentor.LastName,
                IconUrl = _webHostService.GetWebIconUrl(),
                RegistrationUrl = _webHostService.GetRegistrationUrl(token)
            };

            var body = await _razorViewToStringRenderer.RenderViewToStringAsync(Templates.MentorInviteView,
                mentorInviteEmailModel);

            await _emailSenderService.SendEmail(new EmailMessage(new List<string> {mentor.Email},
                "Student mentor registration", body));
        }
    }
}
