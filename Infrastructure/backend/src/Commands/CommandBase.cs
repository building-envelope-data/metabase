using Infrastructure.ValueObjects;
namespace Infrastructure.Commands
{
    public abstract class CommandBase<TResponse>
      : ICommand<TResponse>
    {
        public Id CreatorId { get; }

        public CommandBase(Id creatorId)
        {
            CreatorId = creatorId;
        }
    }
}