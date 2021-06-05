using Microsoft.AspNetCore.Http;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Domain.Services.Implementations
{
    public class WebHostService : IWebHostService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebHostService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetWebIconUrl()
        {
            return $"{GetRootUrl()}/images/student-mentor.svg";
        }

        public string GetRegistrationUrl(string token)
        {
            return $"{GetRootUrl()}/register/{token}";
        }

        private string GetRootUrl()
        {
            return $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}{_httpContextAccessor.HttpContext?.Request.PathBase}";
        }
    }
}
