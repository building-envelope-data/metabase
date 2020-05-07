using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetForwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>
    {
        private GetForwardAssociatesOfModelsIdentifiedByTimestampedIds(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetForwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetForwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetForwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}