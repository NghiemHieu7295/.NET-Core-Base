using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OMI.Core.Mapper
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      var assemblies = new List<Assembly>();
      string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      foreach (var path in Directory.GetFiles(assemblyFolder, "*.dll"))
      {
        ApplyMappingsFromAssembly(Assembly.LoadFrom(path));
      }
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
      var types = assembly.GetExportedTypes()
          .Where(t => t.GetInterfaces().Any(i =>
              i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
          .ToList();

      foreach (var type in types)
      {
        var instance = Activator.CreateInstance(type);
        var methodInfo = type.GetMethod("Mapping");
        methodInfo?.Invoke(instance, new object[] { this });
      }
    }
  }
}
