using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
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