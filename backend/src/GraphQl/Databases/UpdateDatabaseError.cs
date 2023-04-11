using System.Collections.Generic;

namespace Metabase.GraphQl.Databases
{
    public sealed class UpdateDatabaseError
      : GraphQl.UserErrorBase<UpdateDatabaseErrorCode>
    {
        public UpdateDatabaseError(
            UpdateDatabaseErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}