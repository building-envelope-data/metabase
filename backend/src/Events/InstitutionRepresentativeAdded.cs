using CSharpFunctionalExtensions;
using Icon.Infrastructure.Events;
using Newtonsoft.Json;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class InstitutionRepresentativeAdded
      : AssociationAddedEvent
    {
        public static InstitutionRepresentativeAdded From(
            Guid institutionRepresentativeId,
            Commands.AddAssociation<ValueObjects.AddInstitutionRepresentativeInput> command
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

        [JsonIgnore]
        public Guid InstitutionId { get => ParentId; }

        [JsonIgnore]
        public Guid UserId { get => AssociateId; }

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