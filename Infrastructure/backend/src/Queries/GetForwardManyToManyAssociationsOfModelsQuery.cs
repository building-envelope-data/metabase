using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public sealed class GetForwardManyToManyAssociationsOfModelsQuery<TModel, TAssociationModel>
      : GetManyToManyAssociationsOfModelsQuery<TModel, TAssociationModel>
    {
        private GetForwardManyToManyAssociationsOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardManyToManyAssociationsOfModelsQuery<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetForwardManyToManyAssociationsOfModelsQuery<TModel, TAssociationModel>, Errors>(
                new GetForwardManyToManyAssociationsOfModelsQuery<TModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}