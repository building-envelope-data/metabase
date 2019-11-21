using Validatable = Icon.Validatable;
using System;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public class ListComponents
      : Validatable, IQuery<IEnumerable<Result<Models.Component, IError>>>
    {
        public DateTime Timestamp { get; }

        public ListComponents(
            DateTime timestamp
            )
        {
            Timestamp = timestamp;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return Timestamp != DateTime.MinValue;
        }
    }
}