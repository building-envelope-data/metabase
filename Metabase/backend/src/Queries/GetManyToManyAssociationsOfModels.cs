using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public abstract class GetManyToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetAssociationsOfModels<TModel, TAssociationModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        protected GetManyToManyAssociationsOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}