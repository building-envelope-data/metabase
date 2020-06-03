using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class Institution
      : Stakeholder
    {
        public ValueObjects.InstitutionInformation Information { get; }
        public ValueObjects.PublicKey? PublicKey { get; }
        public ValueObjects.InstitutionState State { get; }

        private Institution(
            ValueObjects.Id id,
            ValueObjects.InstitutionInformation information,
            ValueObjects.PublicKey? publicKey,
            ValueObjects.InstitutionState state,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Information = information;
            PublicKey = publicKey;
            State = state;
        }

        public static Result<Institution, Errors> From(
            ValueObjects.Id id,
            ValueObjects.InstitutionInformation information,
            ValueObjects.PublicKey? publicKey,
            ValueObjects.InstitutionState state,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<Institution, Errors>(
                  new Institution(
            id: id,
            information: information,
            publicKey: publicKey,
            state: state,
            timestamp: timestamp
            )
                  );
        }
    }
}