using ValueObjects = Icon.ValueObjects;
using System;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class ListComponents
      : IQuery<IEnumerable<Result<Models.Component, Errors>>>
    {
        public ValueObjects.Timestamp Timestamp { get; }

        private ListComponents(
            ValueObjects.Timestamp timestamp
            )
        {
            Timestamp = timestamp;
        }

        public static Result<ListComponents, Errors> From(
            ValueObjects.Timestamp timestamp
            )
        {
            return Result.Ok<ListComponents, Errors>(
                    new ListComponents(
                      timestamp
                      )
                );
        }
    }
}