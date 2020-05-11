using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetBackwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>
    {
        private GetBackwardAssociatesOfModelsIdentifiedByTimestampedIds(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetBackwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetBackwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetBackwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}