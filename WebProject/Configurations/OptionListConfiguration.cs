using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebProject.Models;

namespace WebProject.Configurations;

public class OptionListConfiguration : IEntityTypeConfiguration<OptionList>
{
    public void Configure(EntityTypeBuilder<OptionList> builder)
    {
        builder.HasKey(x => x.Key);
    }
}
