using System.Collections.Generic;

namespace Metabase.GraphQl.Standards
{
    public abstract class StandardPayload<TStandardError>
      : Payload
      where TStandardError : UserError
    {
        public Data.Standard? Standard { get; }
        public IReadOnlyCollection<TStandardError>? Errors { get; }

        protected StandardPayload(
            Data.Standard standard
            )
        {
            Standard = standard;
        }

        protected StandardPayload(
            IReadOnlyCollection<TStandardError> errors
            )
        {
            Errors = errors;
        }

        protected StandardPayload(
            TStandardError error
            )
          : this(new[] { error })
        {
        }

        protected StandardPayload(
            Data.Standard standard,
            IReadOnlyCollection<TStandardError> errors
            )
        {
            Standard = standard;
            Errors = errors;
        }

        protected StandardPayload(
            Data.Standard standard,
            TStandardError error
            )
          : this(
              standard,
              new[] { error }
              )
        {
        }
    }
}