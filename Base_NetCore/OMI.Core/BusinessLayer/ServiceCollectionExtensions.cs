using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OMI.Core.BusinessLayer
{
  public static class ServiceCollectionExtensions
  {
    public static void RegisterBussinessServices(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
      var assemblies = Assembly.GetCallingAssembly();
      var typesFromAssemblies = assemblies.DefinedTypes.Where(x => x.IsClass && x.GetInterfaces().Any(t => t.IsAssignableFrom(typeof(IBusinessService))));
      foreach (var type in typesFromAssemblies)
        services.Add(new ServiceDescriptor(type.GetInterfaces().FirstOrDefault(), type, lifetime));
    }
  }
}
