using Microsoft.EntityFrameworkCore;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Data.Enums;

namespace StudentMentor.Data.Entities
{
    public class StudentMentorDbContext : DbContext
    {
        public StudentMentorDbContext(DbContextOptions<StudentMentorDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentMentorDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
