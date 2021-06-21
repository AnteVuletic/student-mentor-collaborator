using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Enums;
using StudentMentor.Domain.Mappings;
using StudentMentor.Domain.Models.Configurations;
using StudentMentor.Domain.Models.ViewModels.Account;
using StudentMentor.Domain.Repositories.Implementations;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Implementations;
using StudentMentor.Domain.Services.Interfaces;
using StudentMentor.EmailTemplates;
using StudentMentor.Web.Infrastructure.AuthorizationRequirements;
using StudentMentor.Web.Infrastructure.Email;
using StudentMentor.Web.Infrastructure.Providers;

namespace StudentMentor.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<StudentMentorDbContext>(options => options
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()))
                    .UseSqlServer(configuration.GetConnectionString("StudentMentor")));
            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfiguration = new JwtConfiguration();
            configuration.GetSection(nameof(JwtConfiguration)).Bind(jwtConfiguration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfiguration.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtConfiguration.AudienceId,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.GetAudienceSecretBytes())
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Admin, policy => policy.Requirements.Add(new RoleRequirement(UserRole.Admin)));
                options.AddPolicy(Policies.Mentor, policy => policy.Requirements.Add(new RoleRequirement(UserRole.Mentor)));
                options.AddPolicy(Policies.Student, policy => policy.Requirements.Add(new RoleRequirement(UserRole.Student)));
            });

            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, RoleRequirementHandler>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IClaimProvider, ClaimProvider>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IWebHostService, WebHostService>();
            services.AddTransient<IGithubService, GithubService>();
            services.AddTransient<EmailHandler>();

            services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.Replace(ServiceDescriptor.Singleton<IUserIdProvider, MyUserProvider>());

            return services;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfiguration>(configuration.GetSection(nameof(JwtConfiguration)));
            services.Configure<EmailConfiguration>(configuration.GetSection(nameof(EmailConfiguration)));
            services.Configure<GithubConfiguration>(configuration.GetSection(nameof(GithubConfiguration)));
            services.Configure<HookConfiguration>(configuration.GetSection(nameof(HookConfiguration)));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IMentorRepository, MentorRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IGithubRepository, GithubRepository>();
            services.AddTransient<IFileRepository, FileRepository>();

            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup), typeof(ApplicationProfile));
            return services;
        }

        public static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddTransient<IValidator<RegistrationModel>, RegistrationModelValidation>();
            services.AddTransient<IValidator<MentorInviteModel>, MentorInviteModelValidation>();
            services.AddTransient<IValidator<MentorRegistrationModel>, MentorRegistrationModelValidation>();
            return services;
        }
    }
}
