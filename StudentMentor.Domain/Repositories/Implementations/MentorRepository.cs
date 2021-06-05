using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Helpers;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;
using StudentMentor.Domain.Repositories.Interfaces;

namespace StudentMentor.Domain.Repositories.Implementations
{
    public class MentorRepository : IMentorRepository
    {
        private readonly StudentMentorDbContext _studentMentorDbContext;
        private readonly IMapper _mapper;

        public MentorRepository(
            StudentMentorDbContext studentMentorDbContext,
            IMapper mapper
        ) {
            _studentMentorDbContext = studentMentorDbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<MentorModel>> GetMentors()
        {
            var mentors = await _studentMentorDbContext
                .Mentors
                .ProjectTo<MentorModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return mentors;
        }

        public async Task<MentorModel> GetMentorById(int id)
        {
            var mentor = await _studentMentorDbContext
                .Mentors
                .Where(m => m.Id == id)
                .ProjectTo<MentorModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return mentor;
        }

        public async Task<ResponseResult> DeleteMentor(int mentorId)
        {
            var mentor = await _studentMentorDbContext
                .Mentors
                .FindAsync(mentorId);

            if (mentor == null)
                return ResponseResult.Error("Mentor not found");

            _studentMentorDbContext.Mentors.Remove(mentor);
            await _studentMentorDbContext.SaveChangesAsync();
            return ResponseResult.Ok;
        }

        public async Task<ResponseResult<Mentor>> SetMentorPassword(int mentorId, MentorRegistrationModel model)
        {
            var mentor = await _studentMentorDbContext.Mentors.FindAsync(mentorId);

            if (mentor == null)
                return ResponseResult<Mentor>.Error("Mentor not found");

            mentor.Password = EncryptionHelper.Hash(model.Password);

            await _studentMentorDbContext.SaveChangesAsync();
            return new ResponseResult<Mentor>(mentor);
        }

        public async Task<ResponseResult<Mentor>> CreateMentor(MentorInviteModel model)
        {
            var mentor = _mapper.Map<Mentor>(model);
            await _studentMentorDbContext.Mentors.AddAsync(mentor);
            await _studentMentorDbContext.SaveChangesAsync();
            return new ResponseResult<Mentor>(mentor);
        }
    }
}
