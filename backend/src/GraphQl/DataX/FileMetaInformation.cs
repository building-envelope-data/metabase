using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class FileMetaInformation
    {
        public FileMetaInformation(
          IReadOnlyList<string> path,
          Guid formatId
        )
        {
            Path = path;
            FormatId = formatId;
        }

        public IReadOnlyList<string> Path { get; }
        public Guid FormatId { get; }
    }
}
