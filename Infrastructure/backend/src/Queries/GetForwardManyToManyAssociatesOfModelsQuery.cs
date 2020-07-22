using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public sealed class GetForwardManyToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>
      : GetManyToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
    {
        private GetForwardManyToManyAssociatesOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardManyToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetForwardManyToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetForwardManyToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}