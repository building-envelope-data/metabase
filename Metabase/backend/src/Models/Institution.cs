using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class Institution
      : Stakeholder
    {
        public ValueObjects.InstitutionInformation Information { get; }
        public ValueObjects.PublicKey? PublicKey { get; }
        public ValueObjects.InstitutionState State { get; }

        private Institution(
            Id id,
            ValueObjects.InstitutionInformation information,
            ValueObjects.PublicKey? publicKey,
            ValueObjects.InstitutionState state,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Information = information;
            PublicKey = publicKey;
            State = state;
        }

        public static Result<Institution, Errors> From(
            Id id,
            ValueObjects.InstitutionInformation information,
            ValueObjects.PublicKey? publicKey,
            ValueObjects.InstitutionState state,
            Timestamp timestamp
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