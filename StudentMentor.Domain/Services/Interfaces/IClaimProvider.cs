using StudentMentor.Data.Enums;

namespace StudentMentor.Domain.Services.Interfaces
{
    public interface IClaimProvider
    {
        int GetUserId();
        UserRole GetUserRole();
    }
}
