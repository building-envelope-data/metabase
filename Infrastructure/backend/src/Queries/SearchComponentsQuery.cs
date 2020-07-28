using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.Queries
{
    public sealed class SearchComponentsQuery<TVariable, TResultModel>
      : IQuery<IEnumerable<Result<TResultModel, Errors>>>
    {
        public static Result<SearchComponentsQuery<TVariable, TResultModel>, Errors> From(
            IReadOnlyList<(ValueObjects.Proposition<TVariable>, ValueObjects.Timestamp)> propositionsAndTimestamps
            )
        {
            return Result.Success<SearchComponentsQuery<TVariable, TResultModel>, Errors>(
                new SearchComponentsQuery<TVariable, TResultModel>(
                  propositionsAndTimestamps
                  )
                );
        }

        public IReadOnlyList<(ValueObjects.Proposition<TVariable>, ValueObjects.Timestamp)> PropositionsAndTimestamps { get; }

        private SearchComponentsQuery(
            IReadOnlyList<(ValueObjects.Proposition<TVariable>, ValueObjects.Timestamp)> propositionsAndTimestamps
            )
        {
            PropositionsAndTimestamps = propositionsAndTimestamps;
        }
    }
}
