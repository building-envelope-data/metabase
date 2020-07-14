using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Queries
{
    public sealed class GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetManyToManyAssociationsOfModels<TModel, TAssociationModel>
    {
        private GetForwardManyToManyAssociationsOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors>(
                new GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}