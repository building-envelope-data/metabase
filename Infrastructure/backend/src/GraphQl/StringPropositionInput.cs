using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Infrastructure.GraphQl
{
    public sealed class StringPropositionInput
    {
        public string? EqualTo { get; }
        public string? Like { get; }

        public StringPropositionInput(
            string? equalTo,
            string? like
            )
        {
            EqualTo = equalTo;
            Like = like;
        }

        public static
          Result<ValueObjects.AndProposition<TVariable>, Errors>
          Validate<TVariable>(
            StringPropositionInput self,
            TVariable variable,
            IReadOnlyList<object> path
            )
        {
            var equalToResult =
              self.EqualTo is null
              ? null
              : (Result<ValueObjects.EqualToProposition<TVariable, string>, Errors>?)ValueObjects.EqualToProposition<TVariable, string>.From(
                    variable,
                    self.EqualTo,
                    path.Append("equalTo").ToList().AsReadOnly()
                  );
            var likeResult =
              self.Like is null
              ? null
              : (Result<ValueObjects.LikeProposition<TVariable>, Errors>?)ValueObjects.LikePattern.From(
                self.Like,
                path.Append("like").ToList().AsReadOnly()
                )
              .Bind(likePattern =>
                  ValueObjects.LikeProposition<TVariable>.From(
                    variable,
                    likePattern,
                    path.Append("like").ToList().AsReadOnly()
                    )
                  );

            return
              Errors.CombineExistent(
                  equalToResult,
                  likeResult
                  )
              .Bind(_ =>
                  ValueObjects.AndProposition<TVariable>.From(
                    new ValueObjects.Proposition<TVariable>?[]
                    {
                        equalToResult?.Value,
                        likeResult?.Value
                    }
                    .OfType<ValueObjects.Proposition<TVariable>>(), // excludes null values
                    path
                    )
                  );
        }
    }
}