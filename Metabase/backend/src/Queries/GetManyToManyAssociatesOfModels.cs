using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Metabase.Queries
{
    public abstract class GetManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        protected GetManyToManyAssociatesOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}