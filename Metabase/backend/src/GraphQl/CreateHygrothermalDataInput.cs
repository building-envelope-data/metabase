using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class CreateHygrothermalDataInput
    {
        public Id ComponentId { get; }
        public object Data { get; }

        public CreateHygrothermalDataInput(
            Id componentId,
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