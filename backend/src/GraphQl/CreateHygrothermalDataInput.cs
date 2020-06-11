using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using HotChocolate.Types;
using Array = System.Array;
using Console = System.Console;
using DateTime = System.DateTime;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateHygrothermalDataInput
    {
        public ValueObjects.Id ComponentId { get; }
        public object Data { get; }

        public CreateHygrothermalDataInput(
            ValueObjects.Id componentId,
            object data
            )
        {
            ComponentId = componentId;
            Data = data;
        }

        public static Result<ValueObjects.CreateHygrothermalDataInput, Errors> Validate(
            CreateHygrothermalDataInput self,
            IReadOnlyList<object> path
            )
        {
            var dataResult = ValueObjects.HygrothermalDataJson.FromNestedCollections(
                self.Data,
                path.Append("data").ToList().AsReadOnly()
                );

            return
              dataResult.Bind(_ =>
                  ValueObjects.CreateHygrothermalDataInput.From(
                    componentId: self.ComponentId,
                    data: dataResult.Value
                    )
                  );
        }
    }
}