using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Infrastructure.GraphQl
{
    public sealed class StringPropositionInput
    {
        public string EqualTo { get; }
        public string Like { get; }

        public StringPropositionInput(
            string equalTo,
            string like
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
              ValueObjects.EqualToProposition<TVariable, string>.From(
                    variable,
                    self.EqualTo,
                    path.Append("equalTo").ToList().AsReadOnly()
                  );
            var likeResult = ValueObjects.LikePattern.From(
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
              Errors.Combine(
                  equalToResult,
                  likeResult
                  )
              .Bind(_ =>
                  ValueObjects.AndProposition<TVariable>.From(
                    new ValueObjects.Proposition<TVariable>[]
                    {
                        equalToResult.Value,
                        likeResult.Value
                    },
                    path
                    )
                  );
        }
    }
}