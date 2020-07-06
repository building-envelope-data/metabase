using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
#pragma warning disable SA1302
    public interface Node
#pragma warning restore SA1302
    {
        public Id Id { get; }
        public Timestamp Timestamp { get; }
        public Timestamp RequestTimestamp { get; }
    }
}