using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public sealed class GetForwardOneToManyAssociationsOfModelsQuery<TModel, TAssociationModel>
      : GetOneToManyAssociationsOfModelsQuery<TModel, TAssociationModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        private GetForwardOneToManyAssociationsOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardOneToManyAssociationsOfModelsQuery<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetForwardOneToManyAssociationsOfModelsQuery<TModel, TAssociationModel>, Errors>(
                new GetForwardOneToManyAssociationsOfModelsQuery<TModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}