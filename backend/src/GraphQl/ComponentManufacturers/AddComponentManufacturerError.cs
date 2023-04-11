using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentManufacturers
{
    public sealed class AddComponentManufacturerError
      : UserErrorBase<AddComponentManufacturerErrorCode>
    {
        public AddComponentManufacturerError(
            AddComponentManufacturerErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}