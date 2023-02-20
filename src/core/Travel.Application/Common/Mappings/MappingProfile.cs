using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Travel.Application.Common.Mappings;

public class MappingProfile:Profile
{
    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes().Where(x =>
            x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMapFrom<>))).ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");
            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}