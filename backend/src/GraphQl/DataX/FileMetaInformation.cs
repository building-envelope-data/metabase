using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class FileMetaInformation {
      public List<string> Path { get; set; }
      public Guid FormatId { get; set; }
    }
}
