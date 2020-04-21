using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using ValueObjects = Icon.ValueObjects;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Commands
{
    public sealed class AddPersonAffiliation
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.AddPersonAffiliationInput Input { get; }

        private AddPersonAffiliation(
            ValueObjects.AddPersonAffiliationInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<AddPersonAffiliation, Errors> From(
            ValueObjects.AddPersonAffiliationInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<AddPersonAffiliation, Errors>(
                    new AddPersonAffiliation(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}