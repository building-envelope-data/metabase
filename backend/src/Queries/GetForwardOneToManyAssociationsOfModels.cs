using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetOneToManyAssociationsOfModels<TModel, TAssociationModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        private GetForwardOneToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>, Errors>(
                new GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}