namespace StudentMentor.Domain.Services.Interfaces
{
    public interface IWebHostService
    {
        string GetWebIconUrl();
        string GetRegistrationUrl(string token);
        string GetGithubLoginRedirectUrl();
        public string GetRootUrl();
    }
}
