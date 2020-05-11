using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetForwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>
      : GetAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>
    {
        private GetForwardAssociationsOfModelsIdentifiedByTimestampedIds(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetForwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetForwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>, Errors>(
                new GetForwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}