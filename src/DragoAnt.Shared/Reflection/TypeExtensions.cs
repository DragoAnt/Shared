using System;
using System.Linq;
using System.Reflection;

namespace DragoAnt.Shared.Reflection;

public static class TypeExtensions
{
    /// <summary>
    /// Gets humanazed type name
    /// </summary>
    /// <param name="type">Type for humanize name</param>
    /// <param name="fullName">Use name or full name</param>
    /// <returns></returns>
    public static string HumanizeName(this TypeInfo type, bool fullName = false)
    {
        var name = fullName ? type.FullName ?? type.Name : type.Name;
        if (!type.IsGenericType)
        {
            return name;
        }

        if (type.IsValueType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return type.GetGenericArguments()[0].HumanizeName(fullName) + "?";
        }
            
#if NETSTANDARD2_0
            var typeName = name.Substring(0, name.IndexOf('`'));
#else
        var typeName = name[..name.IndexOf('`')];
#endif
        return $"{typeName}<{string.Join(", ", type.GetGenericArguments().Select(t => HumanizeName(t, fullName)))}>";
    }

    /// <summary>
    /// Gets humanazed type name
    /// </summary>
    /// <param name="type">Type for humanize name</param>
    /// <param name="fullName">Use name or full name</param>
    /// <returns></returns>
    public static string HumanizeName(this Type type, bool fullName = false)
    {
        return type.GetTypeInfo().HumanizeName(fullName);
    }
}