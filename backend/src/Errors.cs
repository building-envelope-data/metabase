using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon
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
        From(
            ErrorBuilder.New()
            .SetMessage(message)
            .SetCode(code)
            .SetPath(path)
            .Build()
            );
      }

      public static Errors From(IList<Result<object, IError>> results)
      {
          return From(
              results
              .Where(r => r.IsFailure)
              .Select(r => r.Error)
              );
      }

      public static Errors From(params Result<object, IError>[] results)
      {
          return From(results);
      }

      public static Errors Concat(params Errors[] collections)
      {
          return Errors.From(collections.SelectMany(x => x));
      }

      public bool IsEmpty()
      {
          return Count == 0;
      }

      // Do not call this method directly. It's only meant to be used by the
      // package `CSharpFunctionalExtensions`.
      public ICombine Combine(ICombine value)
      {
          var errors = value as Errors;
          return Concat(this, errors);
      }
  }
}
