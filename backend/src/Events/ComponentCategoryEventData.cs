using Exception = System.Exception;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using DateTime = System.DateTime;

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
            switch (category)
            {
                case ValueObjects.ComponentCategory.MATERIAL:
                    return ComponentCategoryEventData.MATERIAL;
                case ValueObjects.ComponentCategory.LAYER:
                    return ComponentCategoryEventData.LAYER;
                case ValueObjects.ComponentCategory.UNIT:
                    return ComponentCategoryEventData.UNIT;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The category {category} fell through");
        }

        public static ValueObjects.ComponentCategory ToModel(this ComponentCategoryEventData category)
        {
            switch (category)
            {
                case ComponentCategoryEventData.MATERIAL:
                    return ValueObjects.ComponentCategory.MATERIAL;
                case ComponentCategoryEventData.LAYER:
                    return ValueObjects.ComponentCategory.LAYER;
                case ComponentCategoryEventData.UNIT:
                    return ValueObjects.ComponentCategory.UNIT;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The category {category} fell through");
        }
    }
}