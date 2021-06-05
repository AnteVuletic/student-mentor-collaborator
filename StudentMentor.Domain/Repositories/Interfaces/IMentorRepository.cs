using System.Collections.Generic;
using System.Threading.Tasks;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;

namespace StudentMentor.Domain.Repositories.Interfaces
{
    public interface IMentorRepository
    {
        Task<ICollection<MentorModel>> GetMentors();
        Task<MentorModel> GetMentorById(int id);
        Task<ResponseResult> DeleteMentor(int mentorId);
        Task<ResponseResult<Mentor>> SetMentorPassword(int mentorId, MentorRegistrationModel model);
        Task<ResponseResult<Mentor>> CreateMentor(MentorInviteModel model);
    }
}
