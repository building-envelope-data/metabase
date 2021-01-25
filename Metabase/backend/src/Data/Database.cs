using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metabase.Data
{
    public sealed class Database
      : Infrastructure.Data.Entity
    {
        [MinLength(1)]
        public string Name { get; private set; }

        [MinLength(1)]
        public string Description { get; private set; }

        [Url]
        public string Locator { get; private set; }

        public Guid OperatorId { get; set; }

        [InverseProperty(nameof(Institution.OperatedDatabases))]
        public Institution Operator { get; set; } = default!;

        public Database(
            string name,
            string description,
            string locator
            )
        {
            Name = name;
            Description = description;
            Locator = locator;
        }
    }
}