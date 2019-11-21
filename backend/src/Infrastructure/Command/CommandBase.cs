using Icon;
using Guid = System.Guid;

namespace Icon.Infrastructure.Command
{
    public abstract class CommandBase<TResponse>
      : Validatable, ICommand<TResponse>
    {
        public Guid CreatorId { get; }

        public CommandBase(Guid creatorId)
        {
            CreatorId = creatorId;
        }

        public override bool IsValid()
        {
            return CreatorId != Guid.Empty;
        }
    }
}