using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class CreateOpticalDataInput
    {
        public ValueObjects.Id ComponentId { get; }
        public object Data { get; }

        public CreateOpticalDataInput(
            ValueObjects.Id componentId,
            object data
            )
        {
            ComponentId = componentId;
            Data = data;
        }

        public static Result<ValueObjects.CreateOpticalDataInput, Errors> Validate(
            CreateOpticalDataInput self,
            IReadOnlyList<object> path
            )
        {
            var dataResult = ValueObjects.OpticalDataJson.FromNestedCollections(
                self.Data,
                path.Append("data").ToList().AsReadOnly()
                );

            return
              dataResult.Bind(_ =>
                  ValueObjects.CreateOpticalDataInput.From(
                    componentId: self.ComponentId,
                    data: dataResult.Value
                    )
                  );
        }
    }
}