using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMentor.Data.Entities.Models.Github;

namespace StudentMentor.Data.Configurations.Github
{
    public class FileLogConfiguration : IEntityTypeConfiguration<FileLog>
    {
        public void Configure(EntityTypeBuilder<FileLog> builder)
        {
            builder.ToTable("FileLogs", "github");

            builder.HasOne(fl => fl.Commit)
                .WithMany(c => c.FileLogs)
                .HasForeignKey(fl => fl.CommitId);
        }
    }
}
