using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class AddInstitutionRepresentativeInput
    {
        public Id InstitutionId { get; }
        public Id UserId { get; }
        public InstitutionRepresentativeRole Role { get; }

        private AddInstitutionRepresentativeInput(
            Id institutionId,
            Id userId,
            InstitutionRepresentativeRole role
            )
        {
            InstitutionId = institutionId;
            UserId = userId;
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
    }
}