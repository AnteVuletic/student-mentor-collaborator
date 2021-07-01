using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMentor.Data.Entities.Models;

namespace StudentMentor.Data.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.HasOne(f => f.Message)
                .WithOne(m => m.File)
                .HasForeignKey<Message>(m => m.FileId)
                .IsRequired(false);
        }
    }
}
