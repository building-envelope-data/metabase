using Console = System.Console;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;
using DateTime = System.DateTime;
using ValueObjects = Icon.ValueObjects;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class CreateComponentInput
    {
        public ComponentInformationInput Information { get; }

        public CreateComponentInput(
            ComponentInformationInput information
            )
        {
            Information = information;
        }

        public static Result<ValueObjects.CreateComponentInput, Errors> Validate(
            CreateComponentInput self,
            IReadOnlyList<object> path
            )
        {
            var informationResult = ComponentInformationInput.Validate(
                self.Information,
                path.Append("information").ToList().AsReadOnly()
                );

            return
              Errors.Combine(
                  informationResult
                  )
              .Bind(_ =>
                  ValueObjects.CreateComponentInput.From(
                    information: informationResult.Value
                    )
                  );
        }
    }
}