using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class InstitutionRepresentativeAdded : Event
    {
        public static InstitutionRepresentativeAdded From(
            Guid institutionRepresentativeId,
            Commands.AddInstitutionRepresentative command
            )
        {
            return new InstitutionRepresentativeAdded(
                institutionRepresentativeId: institutionRepresentativeId,
                institutionId: command.Input.InstitutionId,
                userId: command.Input.UserId,
                role: command.Input.Role.FromModel(),
                creatorId: command.CreatorId
                );
        }

        public Guid InstitutionRepresentativeId { get; set; }
        public Guid InstitutionId { get; set; }
        public Guid UserId { get; set; }
        public InstitutionRepresentativeRoleEventData Role { get; set; }

#nullable disable
        public InstitutionRepresentativeAdded() { }
#nullable enable

        public InstitutionRepresentativeAdded(
            Guid institutionRepresentativeId,
            Guid institutionId,
            Guid userId,
            InstitutionRepresentativeRoleEventData role,
            Guid creatorId
            )
          : base(creatorId)
        {
            InstitutionRepresentativeId = institutionRepresentativeId;
            InstitutionId = institutionId;
            UserId = userId;
            Role = role;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(InstitutionRepresentativeId, nameof(InstitutionRepresentativeId)),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId)),
                  ValidateNonEmpty(UserId, nameof(UserId)),
                  ValidateNonNull(Role, nameof(Role))
                  );
        }
    }
}