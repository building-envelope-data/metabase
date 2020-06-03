using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetBackwardManyToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetManyToManyAssociationsOfModels<TModel, TAssociationModel>
    {
        private GetBackwardManyToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetBackwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors>(
                new GetBackwardManyToManyAssociationsOfModels<TModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}