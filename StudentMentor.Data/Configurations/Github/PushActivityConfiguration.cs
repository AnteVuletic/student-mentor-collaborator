using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMentor.Data.Entities.Models.Github;

namespace StudentMentor.Data.Configurations.Github
{
    public class PushActivityConfiguration : IEntityTypeConfiguration<PushActivity>
    {
        public void Configure(EntityTypeBuilder<PushActivity> builder)
        {
            builder.ToTable("PushActivities", "github");
            builder.HasIndex(x => x.RepositoryId);

            builder.HasMany(x => x.Commits)
                .WithOne(c => c.PushActivity)
                .HasForeignKey(x => x.PushActivityId);
        }
    }
}
