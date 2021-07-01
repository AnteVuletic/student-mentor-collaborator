using Microsoft.EntityFrameworkCore;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Data.Entities.Models.Github;

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
        public DbSet<Commit> Commits { get; set; }
        public DbSet<PushActivity> PushActivities { get; set; }
        public DbSet<FileLog> FileLogs { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<StudentFile> StudentFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentMentorDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
