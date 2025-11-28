using ChatMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatMicroservice.Infrastructure.EntityConfigurations;

public class ChatRoomMemberEntityConfiguration : IEntityTypeConfiguration<ChatRoomMember>
{
    public void Configure(EntityTypeBuilder<ChatRoomMember> builder)
    {
        builder.ToTable("ChatRoomMembers");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.JoinedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder.Property(m => m.IsActive).HasDefaultValue(true);
        builder.Property(m => m.IsAdmin).HasDefaultValue(false);

        builder.HasIndex(m => new { m.ChatRoomId, m.UserId }).IsUnique();
        builder.HasIndex(m => m.UserId);
    }
}