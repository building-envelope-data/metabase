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
        public MethodInformation Information { get; }

        public Method(
            Guid id,
            MethodInformation information,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            Information = information;
        }
    }
}