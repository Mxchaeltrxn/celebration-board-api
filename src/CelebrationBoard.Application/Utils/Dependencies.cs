namespace CelebrationBoard.Application.Utils;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class Dependencies
{
  public static void AddApplicationDependencies(this IServiceCollection services)
  {
    services.AddMediatR(Assembly.GetExecutingAssembly());
  }
}
