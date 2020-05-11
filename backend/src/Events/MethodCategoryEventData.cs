using Exception = System.Exception;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public enum MethodCategoryEventData
    {
        MEASUREMENT,
        CALCULATION
    }

    public static class MethodCategoryEventDataExtensions
    {
        public static MethodCategoryEventData FromModel(this ValueObjects.MethodCategory category)
        {
            switch (category)
            {
                case ValueObjects.MethodCategory.MEASUREMENT:
                    return MethodCategoryEventData.MEASUREMENT;
                case ValueObjects.MethodCategory.CALCULATION:
                    return MethodCategoryEventData.CALCULATION;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The category {category} fell through");
        }

        public static ValueObjects.MethodCategory ToModel(this MethodCategoryEventData category)
        {
            switch (category)
            {
                case MethodCategoryEventData.MEASUREMENT:
                    return ValueObjects.MethodCategory.MEASUREMENT;
                case MethodCategoryEventData.CALCULATION:
                    return ValueObjects.MethodCategory.CALCULATION;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The category {category} fell through");
        }
    }
}