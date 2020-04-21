using Icon;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Uri = System.Uri;
using System.Collections.Generic;
using DateTime = System.DateTime;
using System.Threading.Tasks;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class InstitutionCreated : Event
    {
        public static InstitutionCreated From(
            Guid institutionId,
            Commands.CreateInstitution command
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

        public Guid InstitutionId { get; set; }
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
          : base(creatorId)
        {
            InstitutionId = institutionId;
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
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId)),
                  Information.Validate(),
                  ValidateNonNull(State, nameof(State))
                  );
        }
    }
}