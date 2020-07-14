using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class AddInstitutionRepresentativeInput
      : AddOrRemoveInstitutionRepresentativeInput
    {
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        public AddInstitutionRepresentativeInput(
            Id institutionId,
            Id userId,
            ValueObjects.InstitutionRepresentativeRole role
            )
          : base(
              institutionId: institutionId,
              userId: userId
              )
        {
            Role = role;
        }

        public static Result<ValueObjects.AddInstitutionRepresentativeInput, Errors> Validate(
            AddInstitutionRepresentativeInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.AddInstitutionRepresentativeInput.From(
                    institutionId: self.InstitutionId,
                    userId: self.UserId,
                    role: self.Role
                    );
        }
    }
}