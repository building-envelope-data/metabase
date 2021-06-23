using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class GetHttpsResource {
      public string Description { get; set; }
      public string HashValue { get; set; }
      public Uri Locator { get; set; }
      public Guid FormatId { get; set; }
      public List<FileMetaInformation> ArchivedFilesMetaInformation { get; set; }
    }
}
