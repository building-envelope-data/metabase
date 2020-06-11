using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddInstitutionRepresentativeInput
      : AddOrRemoveInstitutionRepresentativeInput
    {
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        public AddInstitutionRepresentativeInput(
            ValueObjects.Id institutionId,
            ValueObjects.Id userId,
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