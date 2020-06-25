namespace Icon.Infrastructure.Command
{
    public abstract class CommandBase<TResponse>
      : ICommand<TResponse>
    {
        public ValueObjects.Id CreatorId { get; }

        public CommandBase(ValueObjects.Id creatorId)
        {
            CreatorId = creatorId;
        }
    }
}