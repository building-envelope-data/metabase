namespace Icon.GraphQl
{
#pragma warning disable SA1302
    public interface Node
#pragma warning restore SA1302
    {
        public ValueObjects.Id Id { get; }
        public ValueObjects.Timestamp Timestamp { get; }
        public ValueObjects.Timestamp RequestTimestamp { get; }
    }
}