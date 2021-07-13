using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class GetHttpsResource
    {
        public GetHttpsResource(
          string description,
          string hashValue,
          Uri locator,
          Guid formatId
        )
        {
            Description = description;
            HashValue = hashValue;
            Locator = locator;
            FormatId = formatId;
        }

        public string Description { get; }
        public string HashValue { get; }
        public Uri Locator { get; }
        public Guid FormatId { get; }
        // public IReadOnlyList<FileMetaInformation> ArchivedFilesMetaInformation { get; }
    }
}
