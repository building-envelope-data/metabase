using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Uri = System.Uri;
using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public class Method
      : Model
    {
        public ValueObjects.MethodInformation Information { get; }

        private Method(
            ValueObjects.Id id,
            ValueObjects.MethodInformation information,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Information = information;
        }

        public static Result<Method, Errors> From(
            ValueObjects.Id id,
            ValueObjects.MethodInformation information,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<Method, Errors>(
                  new Method(
                    id: id,
                    information: information,
                    timestamp: timestamp
                    )
                  );
        }
    }
}