using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public abstract class GetAssociationsOfModels<TModel, TAssociationModel>
      : GetAssociatesOfModels<TModel, TAssociationModel, TAssociationModel>
    {
        protected GetAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }
    }
}