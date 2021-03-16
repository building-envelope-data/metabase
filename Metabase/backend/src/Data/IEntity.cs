using Guid = System.Guid;

namespace Metabase.Data
{
    public interface IEntity
    {
        public Guid Id { get; }

        public uint xmin { get; } // https://www.npgsql.org/efcore/modeling/concurrency.html
    }
}