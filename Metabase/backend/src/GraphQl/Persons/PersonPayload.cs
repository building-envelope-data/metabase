using System.Collections.Generic;

namespace Metabase.GraphQl.Persons
{
    public abstract class PersonPayload<TPersonError>
      : Payload
      where TPersonError : UserError
    {
        public Data.Person? Person { get; }
        public IReadOnlyCollection<TPersonError>? Errors { get; }

        protected PersonPayload(
            Data.Person person
            )
        {
            Person = person;
        }

        protected PersonPayload(
            IReadOnlyCollection<TPersonError> errors
            )
        {
            Errors = errors;
        }

        protected PersonPayload(
            TPersonError error
            )
          : this(new[] { error })
        {
        }

        protected PersonPayload(
            Data.Person person,
            IReadOnlyCollection<TPersonError> errors
            )
        {
            Person = person;
            Errors = errors;
        }

        protected PersonPayload(
            Data.Person person,
            TPersonError error
            )
          : this(
              person,
              new[] { error }
              )
        {
        }
    }
}