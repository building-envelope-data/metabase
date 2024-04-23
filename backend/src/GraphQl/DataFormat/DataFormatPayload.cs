using System.Collections.Generic;

namespace Metabase.GraphQl.DataFormats
{
    public abstract class DataFormatPayload<TDataFormatError>
        : Payload
        where TDataFormatError : IUserError
    {
        public Data.DataFormat? DataFormat { get; }
        public IReadOnlyCollection<TDataFormatError>? Errors { get; }

        protected DataFormatPayload(
            Data.DataFormat person
        )
        {
            DataFormat = person;
        }

        protected DataFormatPayload(
            IReadOnlyCollection<TDataFormatError> errors
        )
        {
            Errors = errors;
        }

        protected DataFormatPayload(
            TDataFormatError error
        )
            : this(new[] { error })
        {
        }

        protected DataFormatPayload(
            Data.DataFormat person,
            IReadOnlyCollection<TDataFormatError> errors
        )
        {
            DataFormat = person;
            Errors = errors;
        }

        protected DataFormatPayload(
            Data.DataFormat person,
            TDataFormatError error
        )
            : this(
                person,
                new[] { error }
            )
        {
        }
    }
}