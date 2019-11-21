using Guid = System.Guid;
using System.Collections.Generic;
using DateTime = System.DateTime;
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
    public sealed class CreateComponentVersion
      : CommandBase<Result<(Guid Id, DateTime Timestamp), IError>>
    {
        public Guid ComponentId { get; }
        public Models.ComponentInformation Information { get; }

        public CreateComponentVersion(
            Guid componentId,
            Models.ComponentInformation information,
            Guid creatorId
            )
          : base(creatorId)
        {
            ComponentId = componentId;
            Information = information;
        }
    }
}