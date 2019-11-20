using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Commands
{
    public sealed class CreateComponent
      : CommandBase<Result<(Guid Id, DateTime Timestamp), IError>>
    {
        public Models.ComponentInformation Information { get; }

        public CreateComponent(
            Models.ComponentInformation information,
            Guid creatorId
            )
          : base(creatorId)
        {
            Information = information;
        }
    }
}