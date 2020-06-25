using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class RemoveMethodDeveloperInput
      : AddOrRemoveMethodDeveloperInput
    {
        public ValueObjects.Timestamp Timestamp { get; }

        public RemoveMethodDeveloperInput(
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId,
            ValueObjects.Timestamp timestamp
            )
          : base(
              methodId: methodId,
              stakeholderId: stakeholderId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<ValueObjects.RemoveManyToManyAssociationInput<Models.MethodDeveloper>, Errors> Validate(
            RemoveMethodDeveloperInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.RemoveManyToManyAssociationInput<Models.MethodDeveloper>.From(
                    parentId: self.MethodId,
                    associateId: self.StakeholderId,
                    timestamp: self.Timestamp
                  );
        }
    }
}