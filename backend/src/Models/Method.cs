using System.Collections.Generic;
using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public class Method
      : Model
    {
        public Guid InformationId { get; }

        public Method(
            Guid id,
            Guid informationId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            InformationId = informationId;
        }
    }
}