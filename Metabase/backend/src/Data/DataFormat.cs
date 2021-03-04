using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metabase.Data
{
    public sealed class DataFormat
      : Infrastructure.Data.Entity
    {
        [Required]
        [MinLength(1)]
        public string Name { get; private set; }

        [MinLength(1)]
        public string? Extension { get; private set; }

        [Required]
        [MinLength(1)]
        public string Description { get; private set; }

        [Required]
        [MinLength(1)]
        public string MediaType { get; private set; }

        [Url]
        public Uri? SchemaLocator { get; private set; }

        public Standard? Standard { get; set; }

        public Publication? Publication { get; set; }

        [NotMapped]
        public IReference? Reference
        {
            get => Standard is not null ? Standard : Publication;
        }

        public Guid ManagerId { get; set; }

        [InverseProperty(nameof(Institution.ManagedDataFormats))]
        public Institution? Manager { get; set; }

#nullable disable
        public DataFormat()
        {
            // Parameterless constructor is needed by HotChocolate's `UseProjection`
        }
#nullable enable

        public DataFormat(
            string name,
            string? extension,
            string description,
            string mediaType,
            Uri? schemaLocator
            )
        {
            Name = name;
            Extension = extension;
            Description = description;
            MediaType = mediaType;
            SchemaLocator = schemaLocator;
        }
    }
}