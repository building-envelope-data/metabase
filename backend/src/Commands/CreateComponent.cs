using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Models = Icon.Models;

namespace Icon.Commands
{
    public sealed class CreateComponent
      : CommandBase<Models.Component>
    {
        public CreateComponent(Guid creatorId) : base(creatorId) { }
    }
}
