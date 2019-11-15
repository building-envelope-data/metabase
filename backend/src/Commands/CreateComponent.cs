using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

#nullable enable
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
          : base(creatorId) {
            Information = information;
          }
    }
}