using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public abstract class GetManyToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>
    {
        protected GetManyToManyAssociatesOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}