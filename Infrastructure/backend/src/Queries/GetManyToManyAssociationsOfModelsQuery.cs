using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public abstract class GetManyToManyAssociationsOfModelsQuery<TModel, TAssociationModel>
      : GetAssociationsOfModelsQuery<TModel, TAssociationModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        protected GetManyToManyAssociationsOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}