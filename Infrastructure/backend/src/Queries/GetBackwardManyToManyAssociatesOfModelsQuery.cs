using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Queries
{
    public sealed class GetBackwardManyToManyAssociatesOfModelsQuery<TAssociateModel, TAssociationModel, TModel>
      : GetManyToManyAssociatesOfModelsQuery<TAssociateModel, TAssociationModel, TModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
    {
        private GetBackwardManyToManyAssociatesOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardManyToManyAssociatesOfModelsQuery<TAssociateModel, TAssociationModel, TModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetBackwardManyToManyAssociatesOfModelsQuery<TAssociateModel, TAssociationModel, TModel>, Errors>(
                new GetBackwardManyToManyAssociatesOfModelsQuery<TAssociateModel, TAssociationModel, TModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}