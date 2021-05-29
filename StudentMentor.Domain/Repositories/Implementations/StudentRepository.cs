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
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentMentorDbContext _dbContext;
        private readonly IMapper _mapper;

        public StudentRepository(StudentMentorDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResponseResult<Student>> RegisterStudent(RegistrationModel model)
        {
            var password = EncryptionHelper.Hash(model.Password);
            var student = new Student
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = password
            };

            _dbContext.Add(student);
            await _dbContext.SaveChangesAsync();
            return new ResponseResult<Student>(student);
        }

        public async Task<ICollection<StudentModel>> GetStudents()
        {
            var students = await _dbContext.Students
                .AsNoTracking()
                .ProjectTo<StudentModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return students;
        }

        public async Task<ResponseResult<StudentModel>> GetStudent(int studentId)
        {
            var student = await _dbContext.Students
                .Where(s => s.Id == studentId)
                .ProjectTo<StudentModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return student is null
                ? ResponseResult<StudentModel>.Error("Not found")
                : new ResponseResult<StudentModel>(student);
        }

        public async Task<ResponseResult> DeleteStudent(int studentId)
        {
            var student = await _dbContext.Students.FindAsync(studentId);

            if (student is null)
                return ResponseResult.Error("Not found");

            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Ok;
        }

        public async Task<ResponseResult> SetMentor(int userId, int mentorId)
        {
            var student = await _dbContext.Students.FindAsync(userId);
            student.MentorId = mentorId;
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Ok;
        }

        public async Task<ResponseResult> RemoveMentor(int userId)
        {
            var student = await _dbContext.Students.FindAsync(userId);
            student.MentorId = null;
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Ok;
        }
    }
}
