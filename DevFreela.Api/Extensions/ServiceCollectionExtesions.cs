using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using DevFreela.Infrastructure.Auth;
using DevFreela.Infrastructure.Payments;
using DevFreela.Infrastructure.MessageBus;
using DevFreela.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
namespace DevFreela.Api.Extensions
{
  public static class ServiceCollectionExtesions
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
      services.AddScoped<IProjectRepository, ProjectRepository>();
      services.AddScoped<ISkillRepository, SkillRepository>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IPaymentService, PaymentService>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<IMessageBusService, MessageBusService>();
      return services;
    }
  }
}