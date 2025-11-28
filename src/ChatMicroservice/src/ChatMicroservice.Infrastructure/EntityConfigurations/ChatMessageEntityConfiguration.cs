using ChatMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatMicroservice.Infrastructure.EntityConfigurations;

public class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Content).HasMaxLength(2000).IsRequired();
        builder.Property(m => m.SentDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder.Property(m => m.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(m => new { m.ChatRoomId, m.SentDate });
        builder.HasIndex(m => m.SenderUserId);
    }
}