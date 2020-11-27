using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class LikeProposition<TVariable>
      : AtomicProposition<TVariable>
    {
        public static Result<LikeProposition<TVariable>, Errors> From(
            TVariable variable,
            LikePattern likePattern,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<LikeProposition<TVariable>, Errors>(
                new LikeProposition<TVariable>(
                  variable, likePattern
                  )
                );
        }

        public LikePattern LikePattern { get; }

        private LikeProposition(
            TVariable variable,
            LikePattern likePattern
            )
          : base(variable)
        {
            LikePattern = likePattern;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            foreach (var component in base.GetEqualityComponents())
            {
                yield return component;
            }
            yield return LikePattern;
        }

        public override Result<bool, Errors> Evaluate(
            Func<TVariable, object?> getVariableValue
            )
        {
            return
              GetCastedNonNullVariableValue<string>(
                  getVariableValue
                  )
              .Map(value =>
                  LikePattern.Matches(value)
                  );
        }
    }
}