using Guid = System.Guid;

#nullable enable
namespace Icon.Infrastructure.Command
{
    public abstract class CommandBase<TResponse> : ICommand<TResponse>
    {
        public Guid CreatorId { get; }

        public CommandBase(Guid creatorId)
        {
            CreatorId = creatorId;
        }
    }
}