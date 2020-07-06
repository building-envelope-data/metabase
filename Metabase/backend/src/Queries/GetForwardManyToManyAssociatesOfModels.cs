using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
    {
        private GetForwardManyToManyAssociatesOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}