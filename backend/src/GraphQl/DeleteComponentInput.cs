using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Array = System.Array;
using Console = System.Console;
using DateTime = System.DateTime;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class DeleteComponentInput
      : DeleteNodeInput
    {
        public DeleteComponentInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}