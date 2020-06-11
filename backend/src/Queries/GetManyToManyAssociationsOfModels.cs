using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public abstract class GetManyToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetAssociationsOfModels<TModel, TAssociationModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        protected GetManyToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}