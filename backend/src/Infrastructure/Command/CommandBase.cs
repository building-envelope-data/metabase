using Guid = System.Guid;

namespace Icon.Infrastructure.Command
{
    public abstract class CommandBase<TResponse> : ICommand<TResponse>
    {
        public Guid CreatorId { get; private set; }

        public CommandBase(Guid creatorId)
        {
            CreatorId = creatorId;
        }
    }
}