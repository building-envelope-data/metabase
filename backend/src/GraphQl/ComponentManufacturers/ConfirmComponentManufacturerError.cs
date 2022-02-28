using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentManufacturers
{
    public sealed class ConfirmComponentManufacturerError
      : UserErrorBase<ConfirmComponentManufacturerErrorCode>
    {
        public ConfirmComponentManufacturerError(
            ConfirmComponentManufacturerErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}