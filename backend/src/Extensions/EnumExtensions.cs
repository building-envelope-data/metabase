using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Metabase.Extensions;

public static class EnumExtensions
{
    [Pure]
    public static string GetDescription(this Enum value)
    {
        return value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DisplayAttribute>()
            ?.Description ?? value.ToString();
    }
}