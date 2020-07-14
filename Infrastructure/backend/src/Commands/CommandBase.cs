using Infrastructure.ValueObjects;
namespace Infrastructure.Commands
{
    public abstract class CommandBase<TResponse>
      : ICommand<TResponse>
    {
        public Id CreatorId { get; }

        protected CommandBase(Id creatorId)
        {
            CreatorId = creatorId;
        }
    }
}