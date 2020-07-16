using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class Method
      : Model
    {
        public ValueObjects.MethodInformation Information { get; }

        private Method(
            Id id,
            ValueObjects.MethodInformation information,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Information = information;
        }

        public static Result<Method, Errors> From(
            Id id,
            ValueObjects.MethodInformation information,
            Timestamp timestamp
            )
        {
            return
              Result.Success<Method, Errors>(
                  new Method(
                    id: id,
                    information: information,
                    timestamp: timestamp
                    )
                  );
        }
    }
}