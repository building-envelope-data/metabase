using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Queries;

namespace Icon.Queries
{
    public sealed class GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
    {
        private GetForwardOneToManyAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}