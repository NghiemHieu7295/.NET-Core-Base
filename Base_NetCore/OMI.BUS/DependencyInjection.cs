using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OMI.Core.BusinessLayer;
using Scrutor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace OMI.BUS
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddBussiness(this IServiceCollection services)
    {
      services.AddMemoryCache();

      var assemblies = new List<Assembly>();
      string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      foreach (var path in Directory.GetFiles(assemblyFolder, "*.dll"))
      {
        assemblies.Add(Assembly.LoadFrom(path));
      }
      services.AddAutoMapper(assemblies);

      services.RegisterBussinessServices();

      return services;
    }
  }
}
