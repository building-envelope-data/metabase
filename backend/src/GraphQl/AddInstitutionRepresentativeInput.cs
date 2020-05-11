using System.Collections.Generic;
using System.Linq;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddInstitutionRepresentativeInput
    {
        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.Id UserId { get; }
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        private AddInstitutionRepresentativeInput(
            ValueObjects.Id institutionId,
            ValueObjects.Id userId,
            ValueObjects.InstitutionRepresentativeRole role
            )
        {
            InstitutionId = institutionId;
            UserId = userId;
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