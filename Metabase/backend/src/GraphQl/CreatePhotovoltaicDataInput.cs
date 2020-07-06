using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class CreatePhotovoltaicDataInput
    {
        public Id ComponentId { get; }
        public object Data { get; }

        public CreatePhotovoltaicDataInput(
            Id componentId,
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