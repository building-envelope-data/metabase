using System.Collections.Generic;

namespace Metabase.GraphQl.Components
{
    public sealed class UpdateComponentPayload
      : ComponentPayload<UpdateComponentError>
    {
        public UpdateComponentPayload(
            Data.Component component
            )
              : base(component)
        {
        }

        public UpdateComponentPayload(
          UpdateComponentError error
        )
        : base(error)
        {
        }

        public UpdateComponentPayload(
          IReadOnlyCollection<UpdateComponentError> errors
        )
        : base(errors)
        {
        }
    }
}