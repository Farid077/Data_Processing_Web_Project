using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebProject.Models;

namespace WebProject.Configurations;

public class BlockListConfiguration : IEntityTypeConfiguration<BlockList>
{
    public void Configure(EntityTypeBuilder<BlockList> builder)
    {
        builder.HasKey(x => x.Key);
    }
}
