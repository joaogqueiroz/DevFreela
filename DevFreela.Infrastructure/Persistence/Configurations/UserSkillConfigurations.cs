using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevFreela.Core.Entities;
namespace DevFreela.Infrastructure.Persistence.Configurations
{
  public class UserSkillConfigurations : IEntityTypeConfiguration<UserSkill>
  {
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
      builder
        .HasKey(p => p.Id);
    }
  }
}