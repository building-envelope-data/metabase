using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class CreateCalorimetricDataInput
    {
        public ValueObjects.Id ComponentId { get; }
        public object Data { get; }

        public CreateCalorimetricDataInput(
            ValueObjects.Id componentId,
            object data
            )
        {
            ComponentId = componentId;
            Data = data;
        }

        public static Result<ValueObjects.CreateCalorimetricDataInput, Errors> Validate(
            CreateCalorimetricDataInput self,
            IReadOnlyList<object> path
            )
        {
            var dataResult = ValueObjects.CalorimetricDataJson.FromNestedCollections(
                self.Data,
                path.Append("data").ToList().AsReadOnly()
                );

            return
              dataResult.Bind(_ =>
                  ValueObjects.CreateCalorimetricDataInput.From(
                    componentId: self.ComponentId,
                    data: dataResult.Value
                    )
                  );
        }
    }
}