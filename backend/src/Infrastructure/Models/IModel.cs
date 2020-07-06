// Value objects are inspired by https://enterprisecraftsmanship.com/posts/value-object-better-implementation/
// For the distinction between value objects and entities (what we call models) see https://enterprisecraftsmanship.com/posts/entity-vs-value-object-the-ultimate-list-of-differences/

namespace Icon.Infrastructure.Models
{
    public interface IModel
    {
        public ValueObjects.Id Id { get; }
        public ValueObjects.Timestamp Timestamp { get; }
    }
}