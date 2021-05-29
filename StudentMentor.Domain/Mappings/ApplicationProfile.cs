using System.Linq;
using AutoMapper;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Models.ViewModels;

namespace StudentMentor.Domain.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<Student, StudentModel>()
                .IncludeBase<User, UserModel>()
                .ForMember(src => src.Mentor, opts => opts.MapFrom(s => s.MentorId.HasValue
                    ? new UserModel
                    {
                        Id = s.Mentor.Id,
                        Email = s.Mentor.Email,
                        FirstName = s.Mentor.FirstName,
                        LastName = s.Mentor.LastName
                    }
                    : null));

            CreateMap<Mentor, MentorModel>()
                .IncludeBase<User, UserModel>()
                .ForMember(src => src.Students, opts => opts.MapFrom(m => m.Students.Select(s => new UserModel
                {
                    Id = s.Id,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    LastName = s.LastName
                })));
        }
    }
}
