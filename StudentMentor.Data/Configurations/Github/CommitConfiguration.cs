using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMentor.Data.Entities.Models.Github;

namespace StudentMentor.Data.Configurations.Github
{
    public class CommitConfiguration : IEntityTypeConfiguration<Commit>
    {
        public void Configure(EntityTypeBuilder<Commit> builder)
        {
            builder.ToTable("Commits", "github");

            builder.HasOne(c => c.PushActivity)
                .WithMany(pa => pa.Commits)
                .HasForeignKey(c => c.PushActivityId);

            builder.HasMany(c => c.FileLogs)
                .WithOne(fl => fl.Commit)
                .HasForeignKey(c => c.CommitId);
        }
    }
}
