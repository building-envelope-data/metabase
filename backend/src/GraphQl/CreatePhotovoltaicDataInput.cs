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
    public sealed class CreatePhotovoltaicDataInput
    {
        public ValueObjects.Id ComponentId { get; }
        public object Data { get; }

        public CreatePhotovoltaicDataInput(
            ValueObjects.Id componentId,
            object data
            )
        {
            ComponentId = componentId;
            Data = data;
        }

        public static Result<ValueObjects.CreatePhotovoltaicDataInput, Errors> Validate(
            CreatePhotovoltaicDataInput self,
            IReadOnlyList<object> path
            )
        {
            var dataResult = ValueObjects.PhotovoltaicDataJson.FromNestedCollections(
                self.Data,
                path.Append("data").ToList().AsReadOnly()
                );

            return
              dataResult.Bind(_ =>
                  ValueObjects.CreatePhotovoltaicDataInput.From(
                    componentId: self.ComponentId,
                    data: dataResult.Value
                    )
                  );
        }
    }
}