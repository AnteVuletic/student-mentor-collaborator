using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMentor.Data.Entities;

namespace StudentMentor.Data.Configurations
{
    public class StudentFileConfiguration : IEntityTypeConfiguration<StudentFile>
    {
        public void Configure(EntityTypeBuilder<StudentFile> builder)
        {
            builder.HasOne(sf => sf.File)
                .WithOne(f => f.StudentFile)
                .HasForeignKey<StudentFile>(sf => sf.FileId);

            builder.HasOne(sf => sf.Student)
                .WithMany(s => s.FinalPapers)
                .HasForeignKey(sf => sf.StudentId);
        }
    }
}
