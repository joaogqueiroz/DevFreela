using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevFreela.Core.Entities;
namespace DevFreela.Infrastructure.Persistence.Configurations
{
  public class SkillConfigurations : IEntityTypeConfiguration<Skill>
  {
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
      builder
        .HasKey(p => p.Id);
    }

  }
}