using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public abstract class GetOneToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>
    {
        protected GetOneToManyAssociatesOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}