using System.Collections.Generic;
using System.Linq;
using Errors = Icon.Errors;
using Guid = System.Guid;
using ValueObjects = Icon.ValueObjects;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddInstitutionRepresentativeInput
    {
        public Guid InstitutionId { get; }
        public Guid UserId { get; }
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        private AddInstitutionRepresentativeInput(
            Guid institutionId,
            Guid userId,
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
            var institutionIdResult = ValueObjects.Id.From(
                self.InstitutionId,
                path.Append("institutionId").ToList().AsReadOnly()
                );
            var userIdResult = ValueObjects.Id.From(
                self.UserId,
                path.Append("userId").ToList().AsReadOnly()
                );

            return
              Errors.CombineExistent(
                  institutionIdResult,
                  userIdResult
                  )
              .Bind(_ =>
                  ValueObjects.AddInstitutionRepresentativeInput.From(
                    institutionId: institutionIdResult.Value,
                    userId: userIdResult.Value,
                    role: self.Role
                    )
                  );
        }
    }
}