using Exception = System.Exception;

namespace Icon.Events
{
    public enum ComponentCategoryEventData
    {
        MATERIAL,
        LAYER,
        UNIT
    }

    public static class ComponentCategoryEventDataExtensions
    {
        public static ComponentCategoryEventData FromModel(this ValueObjects.ComponentCategory category)
        {
            return category switch
            {
                ValueObjects.ComponentCategory.MATERIAL
                    => ComponentCategoryEventData.MATERIAL,
                ValueObjects.ComponentCategory.LAYER
                    => ComponentCategoryEventData.LAYER,
                ValueObjects.ComponentCategory.UNIT
                    => ComponentCategoryEventData.UNIT,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The category {category} fell through")
            };
        }

        public static ValueObjects.ComponentCategory ToModel(this ComponentCategoryEventData category)
        {
            return category switch
            {
                ComponentCategoryEventData.MATERIAL
                    => ValueObjects.ComponentCategory.MATERIAL,
                ComponentCategoryEventData.LAYER
                    => ValueObjects.ComponentCategory.LAYER,
                ComponentCategoryEventData.UNIT
                    => ValueObjects.ComponentCategory.UNIT,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The category {category} fell through")
            };
        }
    }
}