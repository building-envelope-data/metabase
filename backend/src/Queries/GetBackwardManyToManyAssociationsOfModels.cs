using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>
      : GetManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>
    {
        private GetBackwardManyToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>, Errors>(
                new GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}