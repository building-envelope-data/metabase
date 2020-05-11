using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetBackwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>
      : GetAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>
    {
        private GetBackwardAssociationsOfModelsIdentifiedByTimestampedIds(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetBackwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetBackwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>, Errors>(
                new GetBackwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}