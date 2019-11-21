using Exception = System.Exception;
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
        public static ComponentCategoryEventData FromModel(this Models.ComponentCategory category)
        {
            switch (category)
            {
                case Models.ComponentCategory.Material:
                    return ComponentCategoryEventData.Material;
                case Models.ComponentCategory.Layer:
                    return ComponentCategoryEventData.Layer;
                case Models.ComponentCategory.Unit:
                    return ComponentCategoryEventData.Unit;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The category {category} fell through");
        }

        public static Models.ComponentCategory ToModel(this ComponentCategoryEventData category)
        {
            switch (category)
            {
                case ComponentCategoryEventData.Material:
                    return Models.ComponentCategory.Material;
                case ComponentCategoryEventData.Layer:
                    return Models.ComponentCategory.Layer;
                case ComponentCategoryEventData.Unit:
                    return Models.ComponentCategory.Unit;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The category {category} fell through");
        }
    }
}