using Infrastructure.ValueObjects;
// Value objects are inspired by https://enterprisecraftsmanship.com/posts/value-object-better-implementation/
// For the distinction between value objects and entities (what we call models) see https://enterprisecraftsmanship.com/posts/entity-vs-value-object-the-ultimate-list-of-differences/

namespace Infrastructure.Models
{
    public interface IModel
    {
        public Id Id { get; }
        public Timestamp Timestamp { get; }
    }
}