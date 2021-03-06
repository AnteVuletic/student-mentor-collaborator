﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Data.Enums;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Helpers;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Domain.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly StudentMentorDbContext _dbContext;
        private readonly IClaimProvider _claimProvider;
        private readonly IMapper _mapper;

        public UserRepository(StudentMentorDbContext dbContext, IClaimProvider claimProvider, IMapper mapper)
        {
            _dbContext = dbContext;
            _claimProvider = claimProvider;
            _mapper = mapper;
        }

        public async Task<bool> IsEmailAvailable(string email, CancellationToken token)
        {
            var isEmailTaken = await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower().Trim(), token);

            return !isEmailTaken;
        }

        public async Task<ResponseResult<User>> GetUserIfValidCredentials(LoginModel model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email.ToLower().Trim());
            if (user is null)
                return ResponseResult<User>.Error("Invalid password or email");

            if (user.Password is null)
                return ResponseResult<User>.Error("Please set your password using link sent to your email");

            var isValidPassword = EncryptionHelper.ValidatePassword(model.Password, user.Password);
            return isValidPassword
                ? new ResponseResult<User>(user)
                : ResponseResult<User>.Error("Invalid password or email");
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            return user;
        }

        public async Task<UserModel> GetCurrentUserModel()
        {
            var role = _claimProvider.GetUserRole();

            if (role == UserRole.Student)
            {
                var student = await _dbContext.Students
                    .Where(u => u.Id == _claimProvider.GetUserId())
                    .ProjectTo<StudentModel>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();

                return student;
            }

            var user = await _dbContext.Users
                .Where(u => u.Id == _claimProvider.GetUserId())
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return user;
        }
    }
}
