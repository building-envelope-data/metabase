using Guid = System.Guid;

namespace Infrastructure.Data
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        public uint xmin { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html

        protected Entity(
            )
        {
        }
    }
}
