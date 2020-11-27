using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class LikePattern
      : ValueObject
    {
        public string Value { get; }
        private readonly Regex _regex;

        private LikePattern(string value)
        {
            Value = value;
            _regex = Compile(value);
        }

        private Regex Compile(string value)
        {
            // Inspired by https://stackoverflow.com/questions/47052/what-code-would-i-use-to-convert-a-sql-like-expression-to-a-regex-on-the-fly/13180223#13180223
            // TODO What does the case `[...]` do?
            // See https://www.postgresql.org/docs/12/functions-matching.html#FUNCTIONS-LIKE
            return
              new Regex(
                  "^"
                  + Regex.Replace(
                    value,
                    @"[%_]|\[[^]]*\]|[^%_[]+",
                    match =>
                    match.Value switch
                    {
                        "%" => ".*",
                        "_" => ".",
                        var x when x.StartsWith("[") && x.EndsWith("]") => x,
                        var x => Regex.Escape(x)
                    }
                    )
                  + "$",
                  RegexOptions.Compiled, // to improve speed, see https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions?view=netcore-3.1
                  Regex.InfiniteMatchTimeout // otherwise calls to `IsMatch` may throw `RegexMatchTimeoutException` errors, see https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.ismatch?view=netcore-3.1#System_Text_RegularExpressions_Regex_IsMatch_System_String_
                  );
        }

        public static Result<LikePattern, Errors> From(
            string likePattern,
            IReadOnlyList<object>? path = null
            )
        {
            if (likePattern.Length == 0)
            {
                return Result.Failure<LikePattern, Errors>(
                    Errors.One(
                    message: "LikePattern is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Success<LikePattern, Errors>(
                new LikePattern(likePattern)
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public bool Matches(string value)
        {
            return _regex.IsMatch(value);
        }

        public static explicit operator LikePattern(string likePattern)
        {
            return From(likePattern).Value;
        }

        public static implicit operator string(LikePattern likePattern)
        {
            return likePattern.Value;
        }
    }
}