using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Array = System.Array;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using ErrorCodes = Icon.ErrorCodes;
using IError = HotChocolate.IError;

namespace Icon.ValueObjects
{
    public sealed class Numeration
      : ValueObject
    {
        public Prefix? Prefix { get; }
        public MainNumber MainNumber { get; }
        public Suffix? Suffix { get; }

        private Numeration(
            Prefix? prefix,
            MainNumber mainNumber,
            Suffix? suffix
            )
        {
            Prefix = prefix;
            MainNumber = mainNumber;
            Suffix = suffix;
        }

        public static Result<Numeration, Errors> From(
            string? prefix,
            string mainNumber,
            string? suffix,
            IReadOnlyList<object>? path = null
            )
        {
            var prefixResult = Prefix.MaybeFrom(prefix, path: path);
            var mainNumberResult = MainNumber.From(mainNumber, path: path);
            var suffixResult = Suffix.MaybeFrom(suffix, path: path);

            return Errors.CombineExistent(
                prefixResult,
                mainNumberResult,
                suffixResult
                )
          .Bind(_ =>
              From(
                prefixResult?.Value,
                mainNumberResult.Value,
                suffixResult?.Value
                )
              );
        }

        public static Result<Numeration, Errors> From(
            Prefix? prefix,
            MainNumber mainNumber,
            Suffix? suffix,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Ok<Numeration, Errors>(
                new Numeration(
                  prefix,
                  mainNumber,
                  suffix
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Prefix;
            yield return MainNumber;
            yield return Suffix;
        }

        public static explicit operator Numeration(
            (string? Prefix, string MainNumber, string? Suffix) numeration
            )
        {
            return From(numeration.Prefix, numeration.MainNumber, numeration.Suffix).Value;
        }

        public static implicit operator (string?, string, string?)(Numeration numeration)
        {
            return (numeration.Prefix?.Value, numeration.MainNumber, numeration.Suffix?.Value);
        }

        // https://docs.microsoft.com/en-us/dotnet/csharp/tuples#deconstructing-user-defined-types
        public void Deconstruct(out string? prefix, out string mainNumber, out string? suffix)
        {
            prefix = Prefix?.Value;
            mainNumber = MainNumber;
            suffix = Suffix?.Value;
        }

        public static explicit operator Numeration(
            (Prefix? Prefix, MainNumber MainNumber, Suffix? Suffix) numeration
            )
        {
            return From(numeration.Prefix, numeration.MainNumber, numeration.Suffix).Value;
        }

        public static implicit operator (Prefix?, MainNumber, Suffix?)(Numeration numeration)
        {
            return (numeration.Prefix, numeration.MainNumber, numeration.Suffix);
        }

        // https://docs.microsoft.com/en-us/dotnet/csharp/tuples#deconstructing-user-defined-types
        public void Deconstruct(out Prefix? prefix, out MainNumber mainNumber, out Suffix? suffix)
        {
            prefix = Prefix;
            mainNumber = MainNumber;
            suffix = Suffix;
        }
    }
}