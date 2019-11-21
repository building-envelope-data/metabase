using System.Collections.Generic;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public class MethodVersion
      : Model
    {
        public Guid MethodId { get; }
        public MethodInformation Information { get; }

        public MethodVersion(
            Guid id,
            Guid methodId,
            MethodInformation information,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            MethodId = methodId;
            Information = information;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              base.IsValid() &&
              MethodId != Guid.Empty &&
              Information.IsValid();
        }
    }
}