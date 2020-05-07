using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public abstract class GetAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>
      : GetAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociationModel>
    {
        protected GetAssociationsOfModelsIdentifiedByTimestampedIds(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }
    }
}