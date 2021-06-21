using Microsoft.AspNetCore.SignalR;
using StudentMentor.Domain.Constants;

namespace StudentMentor.Web.Infrastructure.Providers
{
    public class MyUserProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userIdString = connection.User?.FindFirst(Claims.UserId)?.Value;

            return userIdString;
        }
    }
}
