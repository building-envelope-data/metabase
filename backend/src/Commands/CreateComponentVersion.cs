using Guid = System.Guid;
using DateTime = System.DateTime;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
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

        public CreateComponentVersion(Guid componentId, Guid creatorId)
          : base(creatorId)
        {
            ComponentId = componentId;
        }
    }
}