using System.Collections.Generic;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public class MethodVersion
      : Model
    {
        public Guid MethodId { get; }
        public Guid InformationId { get; }

        public MethodVersion(
            Guid id,
            Guid methodId,
            Guid informationId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            MethodId = methodId;
            InformationId = informationId;
        }
    }
}