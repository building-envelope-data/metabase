using ValueObjects = Icon.ValueObjects;
using System;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using System.Linq;

namespace Icon.Queries
{
    public abstract class GetManyToManyAssociationsOfModels<TModel, TAssociationModel>
      : IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        protected GetManyToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}