using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class AddComponentVersionInput
      : AddOrRemoveComponentVersionInput
    {
        public AddComponentVersionInput(
            Id baseComponentId,
            Id versionComponentId
            )
          : base(
              baseComponentId: baseComponentId,
              versionComponentId: versionComponentId
              )
        {
        }

        public static Result<ValueObjects.AddComponentVersionInput, Errors> Validate(
            AddComponentVersionInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.AddComponentVersionInput.From(
                    baseComponentId: self.BaseComponentId,
                    versionComponentId: self.VersionComponentId
                  );
        }
    }
}