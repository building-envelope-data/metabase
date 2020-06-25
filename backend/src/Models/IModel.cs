namespace Icon.Models
{
    public interface IModel
    {
        public ValueObjects.Id Id { get; }
        public ValueObjects.Timestamp Timestamp { get; }
    }
}