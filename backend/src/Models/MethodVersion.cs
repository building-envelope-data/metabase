using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public class MethodVersion
      : Model
    {
        public ValueObjects.Id MethodId { get; }
        public ValueObjects.MethodInformation Information { get; }

        private MethodVersion(
            ValueObjects.Id id,
            ValueObjects.Id methodId,
            ValueObjects.MethodInformation information,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            MethodId = methodId;
            Information = information;
        }

        public static Result<MethodVersion, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id methodId,
            ValueObjects.MethodInformation information,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<MethodVersion, Errors>(
                  new MethodVersion(
            id: id,
            methodId: methodId,
            information: information,
            timestamp: timestamp
            )
                  );
        }
    }
}