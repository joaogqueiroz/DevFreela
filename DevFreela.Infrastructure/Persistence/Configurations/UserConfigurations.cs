using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevFreela.Core.Entities;
namespace DevFreela.Infrastructure.Persistence.Configurations
{
  public class UserConfigurations : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {

      builder
        .HasKey(p => p.Id);

      builder
        .HasMany(u => u.Skills)
        .WithOne()
        .HasForeignKey(u => u.IdSkill)
        .OnDelete(DeleteBehavior.Restrict);

    }
  }
}