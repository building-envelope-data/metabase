using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;

namespace Icon.Queries
{
    public sealed class GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>
      : GetManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
    {
        private GetBackwardManyToManyAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>, Errors>(
                new GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}