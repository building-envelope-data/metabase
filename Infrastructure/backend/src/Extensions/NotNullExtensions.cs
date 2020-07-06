// Inspired by
// https://github.com/rmandvikar/csharp-extensions/blob/811ff50f458767d0999b78f5b2bf68fa9434f701/src/rm.Extensions/ThrowExtension.cs
// https://softwareengineering.stackexchange.com/a/336198
// https://code-examples.net/en/q/afd038
// https://github.com/dotnet/csharplang/issues/2477#issuecomment-488576530

using System;

namespace Infrastructure.Extensions
{
    public static class NotNullExtensions
    {
        public static T NotNull<T>(this T? @object, string paramName) where T : class
        {
            if (@object is null)
            {
                throw new InvalidOperationException(paramName);
            }
            return @object!;
        }

        public static T NotNull<T>(this T? @object) where T : class
        {
            return @object.NotNull("unknown");
        }

        public static Guid NotEmpty(this Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidOperationException("Guid is empty");
            }
            return id;
        }

        public static Guid NotEmpty(this Guid? id)
        {
            if (id is null)
            {
                throw new InvalidOperationException("Guid is null");
            }
            return id!.NotEmpty();
        }
    }
}