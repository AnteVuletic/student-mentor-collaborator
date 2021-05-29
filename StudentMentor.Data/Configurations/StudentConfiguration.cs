using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMentor.Data.Entities.Models;

namespace StudentMentor.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
                .HasOne(s => s.Mentor)
                .WithMany(m => m.Students)
                .HasForeignKey(s => s.MentorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
