using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(ti => ti.Id);

        builder.Property(ti => ti.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ti => ti.Description)
            .HasMaxLength(2000);

        builder.Property(ti => ti.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(ti => ti.AssignedTo)
            .WithMany()
            .HasForeignKey(ti => ti.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);


    }
}