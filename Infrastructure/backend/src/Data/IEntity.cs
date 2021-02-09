using Guid = System.Guid;

namespace Infrastructure.Data
{
    public interface IEntity
    {
        public Guid Id { get; }

        public uint xmin { get; } // https://www.npgsql.org/efcore/modeling/concurrency.html
    }
}