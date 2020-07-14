using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Queries
{
    public sealed class GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
    {
        private GetForwardOneToManyAssociatesOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
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