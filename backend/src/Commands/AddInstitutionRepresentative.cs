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
    public sealed class AddInstitutionRepresentative
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.AddInstitutionRepresentativeInput Input { get; }

        private AddInstitutionRepresentative(
            ValueObjects.AddInstitutionRepresentativeInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<AddInstitutionRepresentative, Errors> From(
            ValueObjects.AddInstitutionRepresentativeInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<AddInstitutionRepresentative, Errors>(
                    new AddInstitutionRepresentative(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}