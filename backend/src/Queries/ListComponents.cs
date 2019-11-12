using System;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public class ListComponents :
      IQuery<IEnumerable<Result<Models.Component, IError>>>
    {
        public DateTime Timestamp { get; }

        public ListComponents(
            DateTime timestamp
            )
        {
            Timestamp = timestamp;
        }
    }
}