using Guid = System.Guid;
// using System.ComponentModel.DataAnnotations.Schema;

namespace Metabase.Data
{
    public abstract class Entity
      : IEntity
    {
        public Guid Id { get; set; }

        // [NotMapped]
        // public Guid Uuid { get => Id; }

        public uint xmin { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html

        protected Entity(
            )
        {
        }
    }
}