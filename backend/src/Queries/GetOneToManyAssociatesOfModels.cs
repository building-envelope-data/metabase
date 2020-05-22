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
    public sealed class GetOneToManyAssociatesOfModels<TModel, TAssociateModel>
      : IQuery<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        private GetOneToManyAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<GetOneToManyAssociatesOfModels<TModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetOneToManyAssociatesOfModels<TModel, TAssociateModel>, Errors>(
                new GetOneToManyAssociatesOfModels<TModel, TAssociateModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}
