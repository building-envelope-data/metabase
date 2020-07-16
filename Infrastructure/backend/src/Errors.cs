using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using Errors = Infrastructure.Errors;
using IError = HotChocolate.IError;

namespace Infrastructure
{
    public sealed class Errors
      : ReadOnlyCollection<IError>, ICombine
    {
        private Errors(IList<IError> errors)
          : base(errors)
        {
            if (errors.Count == 0)
                throw new ArgumentException($"The argument errors, that is, {errors}, is empty");
        }

        // Do not call this method directly. It's only meant to be used by the
        // package `CSharpFunctionalExtensions`.
        public ICombine Combine(ICombine value)
        {
            var errors = value as Errors;
            return Concat(this, errors!);
        }

        public override string ToString()
        {
            return string.Join(", ", this.Select(e => $"'{e.Message} ({e.Code})'"));
        }

        public static Errors From(IList<IError> errors)
        {
            return new Errors(errors);
        }

        public static Errors From(IEnumerable<IError> errors)
        {
            return new Errors(errors.ToList());
        }

        public static Errors One(IError error)
        {
            return new Errors(new IError[] { error });
        }

        public static Errors One(
            string message,
            string code,
            IReadOnlyList<object>? path = null
            )
        {
            return One(
                OneX(
                  message: message,
                  code: code,
                  path: path
                  )
                );
        }

        public static IError OneX(
            string message,
            string code,
            IReadOnlyList<object>? path = null
            )
        {
            return
              ErrorBuilder.New()
              .SetMessage(message)
              .SetCode(code)
              .SetPath(path)
              .Build();
        }

        public static Errors ConcatX(IEnumerable<Errors> collections)
        {
            return Errors.From(collections.SelectMany(x => x));
        }

        public static Errors Concat(params Errors[] collections)
        {
            return ConcatX(collections);
        }

        // We would like to use `Result<?, Errors>` here. However, there are no
        // generic wildcards; there is a language proposal though
        // https://github.com/dotnet/csharplang/issues/1992
        // Alternatively, If `Result<.., ..>` implemented an interface
        // `IError<E>` with the method `Error` (similar to the existing interface
        // `IValue<T>`), then we could use that here, which would be terrific.
        // TODO Open a feature request on https://github.com/vkhorikov/CSharpFunctionalExtensions/issues
        // Ask for an interface `IError` (for `IValue` see
        // https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/Result/IResult.cs
        // ) and ask for the implementation of essentially this `Combine`
        // method as an extension method, see
        // https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/Result/Methods/Combine.cs
        public static Result<bool, Errors> CombineX(IEnumerable<IResult> results)
        {
            var errors =
              results
              .Where(r => r.IsFailure)
              .Select(r =>
              {
                  dynamic x = r;
                  return (Errors)x.Error;
              }) // TODO This is super unsafe. Get rid of [`dynamic`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/reference-types#the-dynamic-type) by doing what's been said above
              .ToList();

            if (errors.Count > 0)
                return Result.Failure<bool, Errors>(ConcatX(errors));

            return Result.Success<bool, Errors>(true);
        }

        public static Result<bool, Errors> Combine(params IResult[] results)
        {
            return CombineX(results);
        }

        public static Result<bool, Errors> CombineExistentX(IEnumerable<IResult?> results)
        {
            return CombineX(
                (IEnumerable<IResult>)results.Where(r => !(r is null))
                );
        }

        public static Result<bool, Errors> CombineExistent(params IResult?[] results)
        {
            return CombineExistentX(results);
        }
    }
}