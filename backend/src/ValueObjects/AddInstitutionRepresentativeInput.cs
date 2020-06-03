using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddInstitutionRepresentativeInput
      : AddManyToManyAssociationInput
    {
        public Id InstitutionId { get => ParentId; }
        public Id UserId { get => AssociateId; }
        public InstitutionRepresentativeRole Role { get; }

        private AddInstitutionRepresentativeInput(
            Id institutionId,
            Id userId,
            InstitutionRepresentativeRole role
            )
          : base(
              parentId: institutionId,
              associateId: userId
              )
        {
            Role = role;
        }

        public static Result<AddInstitutionRepresentativeInput, Errors> From(
            Id institutionId,
            Id userId,
            InstitutionRepresentativeRole role
            )
        {
            return
              Result.Ok<AddInstitutionRepresentativeInput, Errors>(
                  new AddInstitutionRepresentativeInput(
                    institutionId: institutionId,
                    userId: userId,
                    role: role
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            foreach (var component in base.GetEqualityComponents())
            {
                yield return component;
            }
            yield return Role;
        }
    }
}