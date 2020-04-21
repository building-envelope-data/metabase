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
    public sealed class CreateInstitution
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.CreateInstitutionInput Input { get; }

        private CreateInstitution(
            ValueObjects.CreateInstitutionInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<CreateInstitution, Errors> From(
            ValueObjects.CreateInstitutionInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<CreateInstitution, Errors>(
                    new CreateInstitution(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}