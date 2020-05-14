using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetBackwardManyToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetManyToManyAssociationsOfModels<TModel, TAssociationModel>
    {
        private GetBackwardManyToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetBackwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetBackwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors>(
                new GetBackwardManyToManyAssociationsOfModels<TModel, TAssociationModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}