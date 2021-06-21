using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMentor.Data.Entities.Models;

namespace StudentMentor.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasOne(m => m.UserFrom)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.UserFromId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(m => m.UserTo)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.UserToId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(m => m.PushActivity)
                .WithOne(pa => pa.Message)
                .HasForeignKey<Message>(x => x.PushActivityId);

            builder.HasOne(m => m.File)
                .WithOne(f => f.Message)
                .HasForeignKey<Message>(m => m.FileId)
                .IsRequired(false);
        }
    }
}
