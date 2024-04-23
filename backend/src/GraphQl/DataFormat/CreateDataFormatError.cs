using System.Collections.Generic;

namespace Metabase.GraphQl.DataFormats
{
    public sealed class CreateDataFormatError
        : GraphQl.UserErrorBase<CreateDataFormatErrorCode>
    {
        public CreateDataFormatError(
            CreateDataFormatErrorCode code,
            string message,
            IReadOnlyList<string> path
        )
            : base(code, message, path)
        {
        }
    }
}