using System;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using StudentMentor.Data.Enums;
using StudentMentor.Domain.Constants;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Domain.Services.Implementations
{
    public class ClaimProvider : IClaimProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext.User.FindFirst(Claims.UserId)?.Value;
            var isSuccessful = int.TryParse(userIdString, out var userId);
            if (!isSuccessful)
                throw new AuthenticationException("Claim non existent");

            return userId;
        }

        public UserRole GetUserRole()
        {
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            var isSuccessful = Enum.TryParse<UserRole>( userRole, out var role);
            if (!isSuccessful)
                throw new AuthenticationException("Claim non existent");

            return role;
        }
    }
}
