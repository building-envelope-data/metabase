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
    public abstract class GetAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>
      : IQuery<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedModelIds { get; }

        protected GetAssociatesOfModelsIdentifiedByTimestampedIds(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            TimestampedModelIds = timestampedModelIds;
        }
    }
}