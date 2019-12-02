using Exception = System.Exception;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public enum ComponentCategoryEventData
    {
        Material,
        Layer,
        Unit
    }

    public static class ComponentCategoryEventDataExtensions
    {
        public static ComponentCategoryEventData FromModel(this ValueObjects.ComponentCategory category)
        {
            switch (category)
            {
                case ValueObjects.ComponentCategory.Material:
                    return ComponentCategoryEventData.Material;
                case ValueObjects.ComponentCategory.Layer:
                    return ComponentCategoryEventData.Layer;
                case ValueObjects.ComponentCategory.Unit:
                    return ComponentCategoryEventData.Unit;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The category {category} fell through");
        }

        public static ValueObjects.ComponentCategory ToModel(this ComponentCategoryEventData category)
        {
            switch (category)
            {
                case ComponentCategoryEventData.Material:
                    return ValueObjects.ComponentCategory.Material;
                case ComponentCategoryEventData.Layer:
                    return ValueObjects.ComponentCategory.Layer;
                case ComponentCategoryEventData.Unit:
                    return ValueObjects.ComponentCategory.Unit;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The category {category} fell through");
        }
    }
}