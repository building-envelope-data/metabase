using Console = System.Console;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;
using DateTime = System.DateTime;
using ValueObjects = Icon.ValueObjects;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class DeleteInstitutionInput
      : DeleteNodeInput
    {
        public DeleteInstitutionInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}