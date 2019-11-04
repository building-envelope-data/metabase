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
    public sealed class CreateComponentVersion : CommandBase<Models.ComponentVersion>
    {
        public CreateComponentVersion(Guid creatorId) : base(creatorId) { }

        public Guid ComponentId { get; set; }
    }
}