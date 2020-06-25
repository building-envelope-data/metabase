using CSharpFunctionalExtensions;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class InstitutionCreated
      : CreatedEvent
    {
        public static InstitutionCreated From(
            Guid institutionId,
            Commands.Create<ValueObjects.CreateInstitutionInput> command
            )
        {
            return new InstitutionCreated(
                institutionId: institutionId,
                information: InstitutionInformationEventData.From(command.Input),
                publicKey: command.Input.PublicKey?.Value,
                state: command.Input.State.FromModel(),
                creatorId: command.CreatorId
                );
        }

        public InstitutionInformationEventData Information { get; set; }
        public string? PublicKey { get; set; }
        public InstitutionStateEventData State { get; set; }

#nullable disable
        public InstitutionCreated() { }
#nullable enable

        public InstitutionCreated(
            Guid institutionId,
            InstitutionInformationEventData information,
            string? publicKey,
            InstitutionStateEventData state,
            Guid creatorId
            )
          : base(
              aggregateId: institutionId,
              creatorId: creatorId
              )
        {
            Information = information;
            PublicKey = publicKey;
            State = state;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  Information.Validate(),
                  ValidateNonNull(State, nameof(State))
                  );
        }
    }
}