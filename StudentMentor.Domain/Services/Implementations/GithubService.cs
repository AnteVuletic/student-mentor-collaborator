using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Octokit;
using Octokit.Internal;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.Configurations;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;
using Repository = Octokit.Repository;

namespace StudentMentor.Domain.Services.Implementations
{
    public class GithubService : IGithubService
    {
        private readonly IWebHostService _webHostService;
        private readonly IStudentRepository _studentRepository;
        private readonly GithubConfiguration _githubConfiguration;
        private readonly HookConfiguration _hookConfiguration;

        private GitHubClient Client => new GitHubClient(new ProductHeaderValue(_githubConfiguration.ClientAppName));

        public GithubService(
            IWebHostService webHostService,
            IStudentRepository studentRepository,
            IOptions<GithubConfiguration> githubOptions,
            IOptions<HookConfiguration> hookOptions
        ) {
            _webHostService = webHostService;
            _studentRepository = studentRepository;
            _githubConfiguration = githubOptions.Value;
            _hookConfiguration = hookOptions.Value;
        }

        public string GetOAuthLink()
        {
            var request = new OauthLoginRequest(_githubConfiguration.ClientId)
            {
                Scopes = { "user", "gist", "repo" },
                RedirectUri = new Uri(_webHostService.GetGithubLoginRedirectUrl())
            };

            var oauthLoginUrl = Client.Oauth.GetGitHubLoginUrl(request);
            return oauthLoginUrl.ToString();
        }

        public async Task<ResponseResult<IReadOnlyCollection<Repository>>> GetAvailableRepositories()
        {
            var client = await ClientWithCredentials();
            try
            {
                var repositories = await client.Repository.GetAllForCurrent(new RepositoryRequest
                { Affiliation = RepositoryAffiliation.Owner });
                return new ResponseResult<IReadOnlyCollection<Repository>>(repositories);
            }
            catch (Exception e)
            {
                return ResponseResult<IReadOnlyCollection<Repository>>.Error(e.StackTrace);
            }
        }

        public async Task<string> GetBearerToken(string oAuthToken = null)
        {
            var oAuthTokenForRequest = "";
            if (oAuthToken == null)
            {
                var response = await _studentRepository.GetOAuthKey();
                oAuthTokenForRequest = response.Data ?? throw new UnauthorizedAccessException();
            }

            if (oAuthToken != null)
            {
                oAuthTokenForRequest = oAuthToken;
            }

            var accessTokenResponse = await Client.Oauth.CreateAccessToken(new OauthTokenRequest(_githubConfiguration.ClientId,
                _githubConfiguration.ClientSecret, oAuthTokenForRequest));
            return accessTokenResponse.AccessToken;
        }

        public async Task<ResponseResult<RepositoryHook>> CreateWebhookForRepositoryId(long repositoryId)
        {
            var client = await ClientWithCredentials();
            try
            {
                var defaultConfiguration = GetDefaultConfiguration("Push");
                var allRepositoryHooks = await client.Repository.Hooks.GetAll(repositoryId);
                var hookForRepository =
                    allRepositoryHooks.FirstOrDefault(hfr => hfr.Url == defaultConfiguration.GetValueOrDefault("url"));
                if (hookForRepository != null)
                    return new ResponseResult<RepositoryHook>(hookForRepository);

                var response = await client.Repository.Hooks.Create(repositoryId, new NewRepositoryHook("web", defaultConfiguration)
                {
                    Events = new List<string>
                    {
                        "push"
                    }
                });

                return new ResponseResult<RepositoryHook>(response);
            }
            catch (Exception e)
            {
                return ResponseResult<RepositoryHook>.Error(e.StackTrace);
            }
        }

        private async Task<GitHubClient> ClientWithCredentials()
        {
            var response = await _studentRepository.GetGitHubAccessToken();
            if (response.Data == null)
                throw new UnauthorizedAccessException();

            return new GitHubClient(new ProductHeaderValue(_githubConfiguration.ClientAppName), new InMemoryCredentialStore(new Credentials(response.Data)));
        }

        private IReadOnlyDictionary<string, string> GetDefaultConfiguration(string method)
        {
            return new Dictionary<string, string>
            {
                { "url", $"{_hookConfiguration.ExposedUrl}/api/Hook/{method}"},
                { "content-type", "application/json"}
            };
        }
    }
}
