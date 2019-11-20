using Guid = System.Guid;

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