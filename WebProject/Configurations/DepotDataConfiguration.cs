using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebProject.Models;

namespace WebProject.Configurations;

public class DepotDataConfiguration : IEntityTypeConfiguration<DepotData>
{
    public void Configure(EntityTypeBuilder<DepotData> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsConfirmed)
            .HasDefaultValue(false);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasOne(x => x.User)
            .WithMany(x => x.ApprovedDepotData)
            .HasForeignKey(x => x.ConfirmerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}