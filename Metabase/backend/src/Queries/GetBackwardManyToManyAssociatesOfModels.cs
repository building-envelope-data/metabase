using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>
      : GetManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>>
    {
        private GetBackwardManyToManyAssociatesOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
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