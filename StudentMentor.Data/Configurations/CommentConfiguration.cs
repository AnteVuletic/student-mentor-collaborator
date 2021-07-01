using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMentor.Data.Entities.Models;

namespace StudentMentor.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(c => c.Message)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MessageId);
        }
    }
}
