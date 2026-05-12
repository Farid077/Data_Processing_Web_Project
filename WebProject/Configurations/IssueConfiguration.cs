using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebProject.Models;

namespace WebProject.Configurations
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(x => x.IsConfirmed)
                .HasDefaultValue(false);

            builder.Property(x => x.Note)
                .HasMaxLength(128)
                .HasDefaultValue("");

            builder.Property(x => x.Status)
                .HasDefaultValue(IssueStatuses.Pending.ToString());

            builder.Property(x => x.Category)
                .HasDefaultValue(Pages.Dashboard.ToString());
            
            builder.Property(x => x.SubCategory)
                .HasDefaultValue(PageAccess.Read_Write.ToString());
        }
    }
}
