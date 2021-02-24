using System.Collections.Generic;

namespace Metabase.GraphQl.Standards
{
    public sealed class CreateStandardError
      : GraphQl.UserErrorBase<CreateStandardErrorCode>
    {
        public CreateStandardError(
            CreateStandardErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}