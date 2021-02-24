using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metabase.Data
{
    public sealed class Database
      : Infrastructure.Data.Entity
    {
        [Required]
        [MinLength(1)]
        public string Name { get; private set; }

        [Required]
        [MinLength(1)]
        public string Description { get; private set; }

        [Required]
        [Url]
        public Uri Locator { get; private set; }

        public Guid OperatorId { get; set; }

        [InverseProperty(nameof(Institution.OperatedDatabases))]
        public Institution? Operator { get; set; }

#nullable disable
        public Database()
        {
            // Parameterless constructor is needed by HotChocolate's `UseProjection`
        }
#nullable enable

        public Database(
            string name,
            string description,
            Uri locator
            )
        {
            Name = name;
            Description = description;
            Locator = locator;
        }
    }
}