using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Queries
{
    public sealed class GetForwardOneToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>
      : GetOneToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
    {
        private GetForwardOneToManyAssociatesOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardOneToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetForwardOneToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetForwardOneToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}