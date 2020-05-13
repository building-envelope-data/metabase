using ValueObjects = Icon.ValueObjects;
using System;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetOneToManyAssociatesOfModels<TModel, TAssociateModel>
      : GetAssociatesOfModels<TModel, TAssociateModel, TAssociateModel>
    {
        private GetOneToManyAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
          : base(timestampedModelIds)
        {
        }

        public static Result<GetOneToManyAssociatesOfModels<TModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetOneToManyAssociatesOfModels<TModel, TAssociateModel>, Errors>(
                new GetOneToManyAssociatesOfModels<TModel, TAssociateModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}