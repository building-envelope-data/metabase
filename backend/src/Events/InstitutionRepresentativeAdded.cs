using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class InstitutionRepresentativeAdded
      : AddedEvent
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

        public Guid InstitutionId { get => ParentId; set => ParentId = value; }
        public Guid UserId { get => AssociateId; set => AssociateId = value; }
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
          : base(
              aggregateId: institutionRepresentativeId,
              parentId: institutionId,
              associateId: userId,
              creatorId: creatorId
              )
        {
            Role = role;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Role, nameof(Role))
                  );
        }
    }
}