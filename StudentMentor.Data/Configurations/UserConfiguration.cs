using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Data.Enums;

namespace StudentMentor.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasDiscriminator(u => u.UserRole)
                .HasValue<Student>(UserRole.Student)
                .HasValue<Mentor>(UserRole.Mentor)
                .HasValue<Admin>(UserRole.Admin);
        }
    }
}
