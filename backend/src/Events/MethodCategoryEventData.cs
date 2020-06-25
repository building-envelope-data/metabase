using Exception = System.Exception;

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
            return category switch
            {
                ValueObjects.MethodCategory.MEASUREMENT
                    => MethodCategoryEventData.MEASUREMENT,
                ValueObjects.MethodCategory.CALCULATION
                    => MethodCategoryEventData.CALCULATION,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The category {category} fell through")
            };
        }

        public static ValueObjects.MethodCategory ToModel(this MethodCategoryEventData category)
        {
            return category switch
            {
                MethodCategoryEventData.MEASUREMENT
                    => ValueObjects.MethodCategory.MEASUREMENT,
                MethodCategoryEventData.CALCULATION
                    => ValueObjects.MethodCategory.CALCULATION,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The category {category} fell through")
            };
        }
    }
}