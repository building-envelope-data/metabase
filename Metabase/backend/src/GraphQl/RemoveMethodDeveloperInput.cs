using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class RemoveMethodDeveloperInput
      : AddOrRemoveMethodDeveloperInput
    {
        public Timestamp Timestamp { get; }

        public RemoveMethodDeveloperInput(
            Id methodId,
            Id stakeholderId,
            Timestamp timestamp
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